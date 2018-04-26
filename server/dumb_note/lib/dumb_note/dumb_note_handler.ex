defmodule DumbNote.DumbNoteServerHandler do
  import DumbNote.DumbNoteServerConnection

  @behaviour DumbNote.DumbNoteServerBehaviour

  def start() do
    :mnesia.start()
    :mnesia.create_table(UserTable, [
      attributes: [:username, :password, :note],
      disc_copies: [node()],
    ])
    #IO.puts "Server started..."
    :ok
  end

  def open_connection(_connection, _socket) do
    #{:ok, {address, port}} = :inet.peername(socket)
    #address_str = :inet.ntoa(address)
    #port_str = Integer.to_string(port)
    #IO.puts ["New client connected: ", address_str, ":", port_str]
    {:ok, {}}
  end

  def close_connection(_connection, _state) do
    #IO.puts "Client disconnected"
    :ok
  end

  def request_register(connection, state, info) do
    #IO.puts "Request Register: " <> info.username <> ", " <> info.password

    read_data = fn -> :mnesia.read(UserTable, info.username) end
    {:atomic, records} = :mnesia.transaction(read_data)

    case records do
      [] ->
        write_data = fn -> :mnesia.write({UserTable, info.username, info.password, ""}) end
        :mnesia.transaction(write_data)
        respond_register(connection, :succeeded)
      _ ->
        respond_register(connection, :duplicate_username)
    end
    {:ok, state}
  end

  def request_login(connection, _state, info) do
    username = info.username
    password = info.password
    #IO.puts "Request Login: " <> username <> ", " <> password

    read_data = fn -> :mnesia.read(UserTable, username) end
    {:atomic, records} = :mnesia.transaction(read_data)

    case records do
      [{UserTable, ^username, ^password, note}] ->
        respond_login(connection, :succeeded)
        update_note(connection, note)
        {:ok, {username, password, note}}
      [{UserTable, ^username, _wrong_password, _note}] ->
        respond_login(connection, :invalid_password)
        {:ok, {}}
      _ ->
        respond_login(connection, :invalid_username)
        {:ok, {}}
    end
  end

  def commit_note(connection, {username, password, note}, new_note) do
    #IO.puts "Commit Note: [" <> username <> "] " <> new_note
    if note != new_note do
      write_data = fn -> :mnesia.write({UserTable, username, password, new_note}) end
      :mnesia.transaction(write_data)
      update_note(connection, new_note)
    end
    {:ok, {username, password, new_note}}
  end
  def commit_note(_connection, state, _new_note) do
    #IO.puts "Commit Note: " <> new_note
    {:ok, state}
  end
end
