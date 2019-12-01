using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public abstract class ConvAdaptorVcgenAbstract<TVertexGenerator, TMarkersGenerator, TVertexSource> : VertexSourceAbstract
		where TVertexGenerator : VertexGeneratorAbstract, new()
		where TMarkersGenerator : MarkersGeneratorAbstract, new()
		where TVertexSource : VertexSourceAbstract
	{
		private readonly TVertexSource _source;

		protected readonly TVertexGenerator Generator = new TVertexGenerator();
		private PathCommand _lastCmd;
		private double _startX;
		private double _startY;
		private Status _status;

		protected ConvAdaptorVcgenAbstract(TVertexSource source) => _source = source;

		public TMarkersGenerator Markers { get; } = new TMarkersGenerator();

		public sealed override void Rewind(int pathId = 0)
		{
			_source.Rewind(pathId);
			_status = Status.Initial;
		}

		public sealed override PathCommand Vertex(ref double x, ref double y)
		{
			var cmd = PathCommand.Stop;
			var done = false;
			while (!done)
			{
				switch (_status)
				{
					case Status.Initial:
						Markers.RemoveAll();
						_lastCmd = _source.Vertex(ref _startX, ref _startY);
						_status = Status.Accumulate;
						break;

					case Status.Accumulate:
						if (_lastCmd.Stop())
						{
							return PathCommand.Stop;
						}

						Generator.RemoveAll();
						Generator.AddVertex(_startX, _startY, PathCommand.MoveTo);
						Markers.AddVertex(_startX, _startY, PathCommand.MoveTo);

						for (;;)
						{
							cmd = _source.Vertex(ref x, ref y);
							if (cmd.Vertex())
							{
								_lastCmd = cmd;
								if (cmd.MoveTo())
								{
									_startX = x;
									_startY = y;
									break;
								}

								Generator.AddVertex(x, y, cmd);
								Markers.AddVertex(x, y, PathCommand.LineTo);
							}
							else
							{
								if (cmd.Stop())
								{
									_lastCmd = PathCommand.Stop;
									break;
								}

								if (!cmd.EndPoly())
								{
									continue;
								}

								Generator.AddVertex(x, y, cmd);
								break;
							}
						}

						Generator.Rewind();
						_status = Status.Generate;
						break;

					case Status.Generate:
						cmd = Generator.Vertex(ref x, ref y);
						if (cmd.Stop())
						{
							_status = Status.Accumulate;
							break;
						}

						done = true;
						break;
				}
			}

			return cmd;
		}

		public sealed override void Dispose()
		{
			Generator?.Dispose();
			Markers?.Dispose();
		}

		private enum Status
		{
			Initial,
			Accumulate,
			Generate
		}
	}
}