node unity DumbNoteClient as C namespace DumbNote	# 客户端
node elixir DumbNoteServer as S namespace DumbNote


enum LoginResult				# 登录结果
	Succeeded					# 成功
	InvalidUsername				# 用户名不存在
	InvalidPassword				# 密码错误
end


enum RegisterResult				# 注册结果
	Succeeded					# 成功
	DuplicateUsername			# 重复用户名已存在
end


struct UserInfo					# 用户信息
	String username				# 用户名
	String password				# 密码
end


direction DumbNoteClient -> DumbNoteServer
	message RequestLogin		# 请求登录
		UserInfo info			# 登录时填写的用户信息
	end

	message RequestRegister		# 请求注册
		UserInfo info			# 注册时填写的用户信息
	end

	message CommitNote			# 提交笔记
		String note				# 笔记内容
	end
end


direction DumbNoteClient <- DumbNoteServer
	message RespondLogin		# 回复登录请求
		LoginResult result		# 登录结果
	end

	message RespondRegister		# 回复注册请求
		RegisterResult result	# 注册结果
	end

	message UpdateNote			# 更新笔记
		String note				# 笔记内容
	end
end


sequence Register				# 注册流程
	C -> S: RequestRegister		# 注册请求
	C <- S: RespondRegister		# 注册回复
end


sequence Login					# 登录流程
	C -> S: RequestLogin		# 登录请求
	C <- S: RespondLogin		# 登录回复
end


sequence NoteCommit				# 笔记提交流程
	C -> S: CommitNote			# 提交笔记
	C <- S: UpdateNote			# 更新笔记
end