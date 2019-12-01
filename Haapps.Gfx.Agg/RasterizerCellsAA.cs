using System.Runtime.CompilerServices;
using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	internal unsafe struct RasterizerCellsAA<TCell>
		where TCell : unmanaged, ICell<TCell>
	{
		private const int QSortThreshold = 9;

		private readonly RefPodList<TCell>* _cells;
		private readonly RefPodArray<PodPtr<TCell>>* _sortedCells;
		private readonly RefPodArray<SortedY>* _sortedY;
		private TCell _currentCell;
		private TCell _styleCell;

		public RasterizerCellsAA(ref RefPodList<TCell> cells, ref RefPodArray<SortedY> sortedY, ref RefPodArray<PodPtr<TCell>> sortedCells) : this()
		{
			_cells = (RefPodList<TCell>*) Unsafe.AsPointer(ref cells);
			_sortedY = (RefPodArray<SortedY>*) Unsafe.AsPointer(ref sortedY);
			_sortedCells = (RefPodArray<PodPtr<TCell>>*) Unsafe.AsPointer(ref sortedCells);
			Reset();
		}

		public bool IsSorted { get; private set; }

		public bool IsEmpty => _cells->Count == 0;

		public int MinX {  get; private set; }

		public int MinY {  get; private set; }

		public int MaxX {  get; private set; }

		public int MaxY {  get; private set; }

		
		public TCell** ScanlineCells(int y, out int count)
		{
			count = (*_sortedY)[y - MinY].Count;
			return (TCell**)_sortedCells->DataPtr + (*_sortedY)[y - MinY].Start;
		}

		public void Reset()
		{
			_cells->Clear();
			_styleCell.Reset();
			_currentCell.Reset();
			MinX = 0x7FFFFFFF;
			MinY = 0x7FFFFFFF;
			MaxX = -0x7FFFFFFF;
			MaxY = -0x7FFFFFFF;
			IsSorted = false;
		}

		 
		private void SetCurrentCell(int x, int y)
		{
			if (!_currentCell.CheckIfNotEqual(x, y))
			{
				return;
			}

			if ((_currentCell.Area | _currentCell.Cover) != 0)
			{
				_cells->Add(ref _currentCell);
			}

			_currentCell.X = x;
			_currentCell.Y = y;
			_currentCell.Cover = 0;
			_currentCell.Area = 0;
		}

		private void RenderHLine(int ey, int x1, int y1, int x2, int y2)
		{
			var ex1 = x1 >> Common.PolySubpixelShift;
			var ex2 = x2 >> Common.PolySubpixelShift;
			var fx1 = x1 & Common.PolySubpixelMask;
			var fx2 = x2 & Common.PolySubpixelMask;

			int delta;

			if (y1 == y2)
			{
				SetCurrentCell(ex2, ey);
				return;
			}

			if (ex1 == ex2)
			{
				delta = y2 - y1;
				_currentCell.Cover += delta;
				_currentCell.Area += (fx1 + fx2) * delta;
				return;
			}

			var p = (Common.PolySubpixelScale - fx1) * (y2 - y1);
			var first = Common.PolySubpixelScale;
			var incr = 1;

			var dx = x2 - x1;

			if (dx < 0)
			{
				p = fx1 * (y2 - y1);
				first = 0;
				incr = -1;
				dx = -dx;
			}

			delta = p / dx;
			var mod = p % dx;

			if (mod < 0)
			{
				delta--;
				mod += dx;
			}

			_currentCell.Cover += delta;
			_currentCell.Area += (fx1 + first) * delta;

			ex1 += incr;
			SetCurrentCell(ex1, ey);

			y1 += delta;

			if (ex1 != ex2)
			{
				p = Common.PolySubpixelScale * (y2 - y1 + delta);
				var lift = p / dx;
				var rem = p % dx;

				if (rem < 0)
				{
					lift--;
					rem += dx;
				}

				mod -= dx;

				while (ex1 != ex2)
				{
					delta = lift;
					mod += rem;
					if (mod >= 0)
					{
						mod -= dx;
						delta++;
					}

					_currentCell.Cover += delta;
					_currentCell.Area += Common.PolySubpixelScale * delta;
					y1 += delta;
					ex1 += incr;
					SetCurrentCell(ex1, ey);
				}
			}

			delta = y2 - y1;
			_currentCell.Cover += delta;
			_currentCell.Area += (fx2 + Common.PolySubpixelScale - first) * delta;
		}

		public void SetStyle(ref TCell cell) => _styleCell.SetStyle(ref cell);

		public void Line(int x1, int y1, int x2, int y2)
		{
			const int dxLimit = 16384 << Common.PolySubpixelShift;

			var dx = x2 - x1;

			if (dx >= dxLimit || dx <= -dxLimit)
			{
				var cx = (x1 + x2) >> 1;
				var cy = (y1 + y2) >> 1;
				Line(x1, y1, cx, cy);
				Line(cx, cy, x2, y2);
			}

			var dy = y2 - y1;
			var ex1 = x1 >> Common.PolySubpixelShift;
			var ex2 = x2 >> Common.PolySubpixelShift;
			var ey1 = y1 >> Common.PolySubpixelShift;
			var ey2 = y2 >> Common.PolySubpixelShift;
			var fy1 = y1 & Common.PolySubpixelMask;
			var fy2 = y2 & Common.PolySubpixelMask;

			int delta, first;

			if (ex1 < MinX)
			{
				MinX = ex1;
			}

			if (ex1 > MaxX)
			{
				MaxX = ex1;
			}

			if (ey1 < MinY)
			{
				MinY = ey1;
			}

			if (ey1 > MaxY)
			{
				MaxY = ey1;
			}

			if (ex2 < MinX)
			{
				MinX = ex2;
			}

			if (ex2 > MaxX)
			{
				MaxX = ex2;
			}

			if (ey2 < MinY)
			{
				MinY = ey2;
			}

			if (ey2 > MaxY)
			{
				MaxY = ey2;
			}

			SetCurrentCell(ex1, ey1);

			if (ey1 == ey2)
			{
				RenderHLine(ey1, x1, fy1, x2, fy2);
				return;
			}

			var incr = 1;
			if (dx == 0)
			{
				var ex = x1 >> Common.PolySubpixelShift;
				var twoFx = (x1 - (ex << Common.PolySubpixelShift)) << 1;

				first = Common.PolySubpixelScale;
				if (dy < 0)
				{
					first = 0;
					incr = -1;
				}

				delta = first - fy1;
				_currentCell.Cover += delta;
				_currentCell.Area += twoFx * delta;

				ey1 += incr;
				SetCurrentCell(ex, ey1);

				delta = first + first - Common.PolySubpixelScale;
				var area = twoFx * delta;
				while (ey1 != ey2)
				{
					_currentCell.Cover = delta;
					_currentCell.Area = area;
					ey1 += incr;
					SetCurrentCell(ex, ey1);
				}

				delta = fy2 - Common.PolySubpixelScale + first;
				_currentCell.Cover += delta;
				_currentCell.Area += twoFx * delta;
				return;
			}

			var p = (Common.PolySubpixelScale - fy1) * dx;
			first = Common.PolySubpixelScale;

			if (dy < 0)
			{
				p = fy1 * dx;
				first = 0;
				incr = -1;
				dy = -dy;
			}

			delta = p / dy;
			var mod = p % dy;

			if (mod < 0)
			{
				delta--;
				mod += dy;
			}

			var xFrom = x1 + delta;
			RenderHLine(ey1, x1, fy1, xFrom, first);

			ey1 += incr;
			SetCurrentCell(xFrom >> Common.PolySubpixelShift, ey1);

			if (ey1 != ey2)
			{
				p = Common.PolySubpixelScale * dx;
				var lift = p / dy;
				var rem = p % dy;

				if (rem < 0)
				{
					lift--;
					rem += dy;
				}

				mod -= dy;

				while (ey1 != ey2)
				{
					delta = lift;
					mod += rem;
					if (mod >= 0)
					{
						mod -= dy;
						delta++;
					}

					var xTo = xFrom + delta;
					RenderHLine(ey1, xFrom, Common.PolySubpixelScale - first, xTo, first);
					xFrom = xTo;

					ey1 += incr;
					SetCurrentCell(xFrom >> Common.PolySubpixelShift, ey1);
				}
			}

			RenderHLine(ey1, xFrom, Common.PolySubpixelScale - first, x2, fy2);
		}

		public void Line(double x1, double y1, double x2, double y2) => Line((int)x1, (int)y1, (int)x2, (int)y2);

		private static void QSortCells(TCell** start, int num, TCell*** stack)
		{
			var limit = start + num;
			var bs = start;
			var top = stack;

			for (;;)
			{
				var len = (int) (limit - bs);

				if (len > QSortThreshold)
				{
					var pivot = bs + len/2;

					var temp = *bs;
					*bs = *pivot;
					*pivot = temp;

					var i = bs + 1;
					var j = limit - 1;

					if ((*j)->X < (*i)->X)
					{
						temp = *i;
						*i = *j;
						*j = temp;
					}

					if ((*bs)->X < (*i)->X)
					{
						temp = *bs;
						*bs = *i;
						*i = temp;
					}

					if ((*j)->X < (*bs)->X)
					{
						temp = *bs;
						*bs = *j;
						*j = temp;
					}

					for (;;)
					{
						var x = (*bs)->X;
						do i++; while ((*i)->X < x);
						do j--; while ((*j)->X > x);

						if (i > j)
						{
							break;
						}
						temp = *i;
						*i = *j;
						*j = temp;
					}

					temp = *bs;
					*bs = *j;
					*j = temp;

					if (j - bs > limit - i)
					{
						top[0] = bs;
						top[1] = j;
						bs = i;
					}
					else
					{
						top[0] = i;
						top[1] = limit;
						limit = j;
					}
					top += 2;
				}
				else
				{
					var j = bs;
					var i = j + 1;

					for (; i < limit; j = i, i++)
					{
						for (; j[1]->X < (*j)->X; j--)
						{
							var temp = *(j + 1);
							*(j + 1) = *j;
							*j = temp;

							if (j == bs)
							{
								break;
							}
						}
					}

					if (top > stack)
					{
						top -= 2;
						bs = top[0];
						limit = top[1];
					}
					else
					{
						break;
					}
				}
			}
		}

		public void SortCells()
		{
			if (IsSorted)
			{
				return;
			}

			if ((_currentCell.Area | _currentCell.Cover) != 0)
			{
				_cells->Add(ref _currentCell);
			}

			_currentCell.X = 0x7FFFFFFF;
			_currentCell.Y = 0x7FFFFFFF;
			_currentCell.Cover = 0;
			_currentCell.Area = 0;

			_sortedCells->Reallocate(_cells->Count);

			_sortedY->Reallocate(MaxY - MinY + 1);
			_sortedY->Zero();

			var cellPtr = _cells->DataPtr;

			var i = _cells->Count;
			while (i-- != 0)
			{
				(*_sortedY)[cellPtr->Y - MinY].Start++;
				++cellPtr;
			}

			var start = 0;
			for (i = 0; i < _sortedY->Size; i++)
			{
				var v = (*_sortedY)[i].Start;
				(*_sortedY)[i].Start = start;
				start += v;
			}

			cellPtr = _cells->DataPtr;
			var sortedCells = (TCell**) _sortedCells->DataPtr;
			i = _cells->Count;
			while (i-- != 0)
			{
				var currY = (*_sortedY).DataPtr + cellPtr->Y - MinY;
				sortedCells[currY->Start + currY->Count] = cellPtr;
				currY->Count++;
				++cellPtr;
			}

			var stack = stackalloc TCell**[80];
			for (i = 0; i < _sortedY->Size; i++)
			{
				var currY = (*_sortedY)[i];
				if (currY.Count == 0)
				{
					continue;
				}

				QSortCells(sortedCells + currY.Start, currY.Count, stack);
			}


			IsSorted = true;
		}
	}
}
