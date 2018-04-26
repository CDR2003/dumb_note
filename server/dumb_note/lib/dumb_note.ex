defmodule DumbNote do
  use Application

  def start(_type, _args) do
    DumbNote.Supervisor.start_link()
  end
end
