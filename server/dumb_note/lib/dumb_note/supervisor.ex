defmodule DumbNote.Supervisor do
  use Supervisor

  def start_link() do
    Supervisor.start_link(__MODULE__, [], [])
  end

  def init(_args) do
    children = [
      {Brotorift.Supervisor, [port: 9000, mod: DumbNote.DumbNoteServerConnection, handler: DumbNote.DumbNoteServerHandler]},
    ]

    Supervisor.init(children, strategy: :one_for_one)
  end
end
