using System;
using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	public sealed unsafe class VcgenDash : VertexGeneratorAbstract
	{
		private const int MaxDashes = 32;
		private readonly PodArray<double> _dashes = new PodArray<double>(MaxDashes);
		private readonly PodList<VertexDistance> _srcVertices = new PodList<VertexDistance>(64);
		private int _closed;
		private int _currDash;
		private double _currDashStart;
		private double _currRest;
		private double _dashStart;
		private int _numDashes;
		private int _srcVertex;
		private Status _status;
		private VertexDistance* _v1;
		private VertexDistance* _v2;

		public double Shorten { get; set; }

		public double TotalDashLenght { get; private set; }

		public void RemoveAllDashes()
		{
			TotalDashLenght = 0.0;
			_numDashes = 0;
			_currDashStart = 0.0;
			_currDash = 0;
		}

		public void AddDash(double dashLen, double gapLen)
		{
			if (_numDashes >= MaxDashes)
			{
				return;
			}

			TotalDashLenght += dashLen + gapLen;
			_dashes[_numDashes++] = dashLen;
			_dashes[_numDashes++] = gapLen;
		}

		public void DashStart(double ds)
		{
			_dashStart = ds;
			CalcDashStart(Math.Abs(ds));
		}

		private void CalcDashStart(double ds)
		{
			_currDash = 0;
			_currDashStart = 0.0;
			while (ds > 0.0)
			{
				if (ds > _dashes[_currDash])
				{
					ds -= _dashes[_currDash];
					++_currDash;
					_currDashStart = 0.0;
					if (_currDash >= _numDashes)
					{
						_currDash = 0;
					}
				}
				else
				{
					_currDashStart = ds;
					ds = 0.0;
				}
			}
		}

		public override void RemoveAll()
		{
			_status = Status.Initial;
			_srcVertices.Clear();
			_closed = 0;
		}

		public override void AddVertex(double x, double y, PathCommand cmd)
		{
			_status = Status.Initial;
			if (cmd.MoveTo())
			{
				if (_srcVertices.Count != 0)
				{
					_srcVertices.RemoveLast();
				}

				AddVertex(x, y);
			}
			else
			{
				if (cmd.Vertex())
				{
					AddVertex(x, y);
				}
				else
				{
					_closed = cmd.GetCloseFlag();
				}
			}
		}

		public override void Rewind(int pathId = 0)
		{
			if (_status == Status.Initial)
			{
				CloseVertexPath(_closed != 0);
				if (Shorten > 0.0 && _srcVertices.Count > 1)
				{
					double d;
					var n = _srcVertices.Count - 2;
					while (n != 0)
					{
						d = _srcVertices[n].Distance;
						if (d > Shorten)
						{
							break;
						}

						if (_srcVertices.Count != 0)
						{
							_srcVertices.RemoveLast();
						}

						Shorten -= d;
						--n;
					}

					if (_srcVertices.Count < 2)
					{
						_srcVertices.Clear();
					}
					else
					{
						n = _srcVertices.Count - 1;
						ref var prev = ref _srcVertices[n - 1];
						ref var last = ref _srcVertices[n];
						d = (prev.Distance - Shorten) / prev.Distance;
						var x = prev.X + (last.X - prev.X) * d;
						var y = prev.Y + (last.Y - prev.Y) * d;
						last.X = x;
						last.Y = y;
						_srcVertices[n] = last;
						_srcVertices[n - 1] = prev;
						if (!prev.Measure(ref last))
						{
							if (_srcVertices.Count != 0)
							{
								_srcVertices.RemoveLast();
							}
						}

						CloseVertexPath(_closed != 0);
					}
				}
			}

			_status = Status.Ready;
			_srcVertex = 0;
		}

		public override PathCommand Vertex(ref double x, ref double y)
		{
			var cmd = PathCommand.MoveTo;
			while (!cmd.Stop())
			{
				switch (_status)
				{
					case Status.Initial:
						Rewind();
						_status = Status.Ready;
						break;

					case Status.Ready:
						if (_numDashes < 2 || _srcVertices.Count < 2)
						{
							cmd = PathCommand.Stop;
							break;
						}

						_status = Status.Polyline;
						_srcVertex = 1;
						_v1 = _srcVertices.DataPtr;
						_v2 = _srcVertices.DataPtr + 1;
						_currRest = _v1->Distance;
						x = _v1->X;
						y = _v1->Y;
						if (_dashStart >= 0.0)
						{
							CalcDashStart(_dashStart);
						}

						return PathCommand.MoveTo;

					case Status.Polyline:
					{
						var dashRest = _dashes[_currDash] - _currDashStart;

						cmd = (_currDash & 1) != 0 ? PathCommand.MoveTo : PathCommand.LineTo;

						if (_currRest > dashRest)
						{
							_currRest -= dashRest;
							++_currDash;
							if (_currDash >= _numDashes)
							{
								_currDash = 0;
							}

							_currDashStart = 0.0;
							x = _v2->X - (_v2->X - _v1->X) * _currRest / _v1->Distance;
							y = _v2->Y - (_v2->Y - _v1->Y) * _currRest / _v1->Distance;
						}
						else
						{
							_currDashStart += _currRest;
							x = _v2->X;
							y = _v2->Y;
							++_srcVertex;
							_v1 = _v2;
							_currRest = _v1->Distance;
							if (_closed != 0)
							{
								if (_srcVertex > _srcVertices.Count)
								{
									_status = Status.Stop;
								}
								else
								{
									_v2 = _srcVertices.DataPtr + (_srcVertex >= _srcVertices.Count ? 0 : _srcVertex);
								}
							}
							else
							{
								if (_srcVertex >= _srcVertices.Count)
								{
									_status = Status.Stop;
								}
								else
								{
									_v2 = _srcVertices.DataPtr + _srcVertex;
								}
							}
						}

						return cmd;
					}

					case Status.Stop:
						cmd = PathCommand.Stop;
						break;
				}
			}

			return PathCommand.Stop;
		}

		private void AddVertex(double x, double y, double dist = 0)
		{
			if (_srcVertices.Count > 1)
			{
				ref var vd1 = ref _srcVertices[_srcVertices.Count - 2];
				ref var vd2 = ref _srcVertices[_srcVertices.Count - 1];
				var ret = vd1.Measure(ref vd2);
				if (!ret)
				{
					if (_srcVertices.Count != 0)
					{
						_srcVertices.RemoveLast();
					}
				}
			}

			_srcVertices.Add(new VertexDistance(x, y, dist));
		}

		private void CloseVertexPath(bool closed)
		{
			while (_srcVertices.Count > 1)
			{
				ref var vd1 = ref _srcVertices[_srcVertices.Count - 2];
				ref var vd2 = ref _srcVertices[_srcVertices.Count - 1];
				var ret = vd1.Measure(ref vd2);
				if (ret)
				{
					break;
				}

				var t = _srcVertices[_srcVertices.Count - 1];
				if (_srcVertices.Count != 0)
				{
					_srcVertices.RemoveLast();
				}

				if (_srcVertices.Count != 0)
				{
					_srcVertices.RemoveLast();
				}

				AddVertex(t.X, t.Y, t.Distance);
			}

			if (!closed)
			{
				return;
			}

			while (_srcVertices.Count > 1)
			{
				ref var vd1 = ref _srcVertices[_srcVertices.Count - 1];
				ref var vd2 = ref _srcVertices[0];
				var ret = vd1.Measure(ref vd2);
				if (ret)
				{
					break;
				}

				if (_srcVertices.Count != 0)
				{
					_srcVertices.RemoveLast();
				}
			}
		}

		public override void Dispose()
		{
			_dashes?.Dispose();
			_srcVertices?.Dispose();
		}

		private enum Status
		{
			Initial,
			Ready,
			Polyline,
			Stop
		}
	}
}