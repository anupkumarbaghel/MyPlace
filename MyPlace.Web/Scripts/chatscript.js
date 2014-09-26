(function () {
    var chthub = $.connection.chatHub;
    //start hub
    $.connection.hub.start();
    $('#usrName').val(prompt('Enter your name:', ''));

    //hub client implementation
    chthub.client.rcvByClient = function (user, message) {
        $("#uldiscussion").append(user + ":" + message);
    };

    //hub server implementation
    $("#sendMessage").click(function () {
        chthub.server.send($("#usrName").val(), $("#writeMsg").val());
    });
}());


