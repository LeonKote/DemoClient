mergeInto(LibraryManager.library, {

	StartClient: function ()
	{
		socket = new WebSocket('wss://rtflegion.ru:8887');

		socket.onopen = function (e)
		{
			myGameInstance.SendMessage('Canvas', 'OnResponse', '{"websocket":"OK"}');
		};

		socket.onmessage = function (e)
		{
			myGameInstance.SendMessage('Canvas', 'OnResponse', e.data);
		};
	},

	Send: function (message)
	{
		socket.send(UTF8ToString(message));
	},
	
	Log: function (message)
	{
		console.log(UTF8ToString(message));
	}

});
