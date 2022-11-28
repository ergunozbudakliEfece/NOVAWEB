function LaunchHeartBeat(actionUrl) {
    var intervalMilliseconds = 750;
    setInterval(function () {
        $.ajax({
            type: "POST",
            url: actionUrl
        });
    }, intervalMilliseconds);
}