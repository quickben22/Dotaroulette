///////////////////////////////////////////////////////////////////////
// Constants


var ProgressBar = function () {
    var that = {};

    // Store the XHR object being used
    that._xhr = null;

    // Get the current task ID
    that._taskId = 0;

    // Get the timer ID used to push refreshes
    that._timerId = 0;

    // Get the URL to invoke to read status
    that._progressUrl = "";

    // Get the URL to invoke to abort the operation 
    that._abortUrl = "";

    // Get the current interval for progress refresh (in ms)
    that._interval = 500;

    // Get the user-defined callback that refreshes the UI 
    that._userDefinedProgressCallback = null;

    // Get the user-defined callback that finalizes the call 
    that._taskCompletedCallback = null;

    // Get the user-defined callback that runs after aborting the call 
    that._taskAbortedCallback = null;


    // Get a new task ID
    that.createTaskId = function () {
        var _minNumber = 100,
            _maxNumber = 1000000000;
        return _minNumber + Math.floor(Math.random() * _maxNumber);
    };

    // Set progress callback
    that.callback = function (userCallback, completedCallback, abortedCallback) {
        that._userDefinedProgressCallback = userCallback;
        that._taskCompletedCallback = completedCallback;
        that._taskAbortedCallback = abortedCallback;
        return this;
    };

    // Set frequency of refresh
    that.setInterval = function (interval) {
        that._interval = interval;
        return this;
    };

    // Abort the operation
    that.abort = function () {
        //        if (_xhr !== null)
        //            _xhr.abort();
        if (that._abortUrl != null && that._abortUrl != "") {
            $.ajax({
                url: that._abortUrl,
                cache: false,
                headers: { 'X-ProgressBar-TaskId': that._taskId }
            });
        }
    };

    // INTERNAL FUNCTION
    that._internalProgressCallback = function () {
        that._timerId = window.setTimeout(that._internalProgressCallback, that._interval);
        $.ajax({
            url: that._progressUrl,
            cache: false,
            headers: { 'X-ProgressBar-TaskId': that._taskId },
            success: function (status) {
                if (that._userDefinedProgressCallback != null)
                    that._userDefinedProgressCallback(status);
            }
        });
    };

    // Invoke the URL and monitor its progress
    that.start = function (url, progressUrl, abortUrl) {
        that._taskId = that.createTaskId();
        that._progressUrl = progressUrl;
        that._abortUrl = abortUrl;

        // Place the Ajax call
        _xhr = $.ajax({
            url: url,
            cache: false,
            headers: { 'X-ProgressBar-TaskId': that._taskId },
            complete: function () {
                if (_xhr.status != 0) return;
                if (that._taskAbortedCallback != null)
                    that._taskAbortedCallback();
                that.end();
            },
            success: function (data) {
                if (that._taskCompletedCallback != null)
                    that._taskCompletedCallback(data);
                that.end();
            }
        });

        // Start the progress callback (if any)
        if (that._userDefinedProgressCallback == null || that._progressUrl === "")
            return this;
        that._timerId = window.setTimeout(that._internalProgressCallback, that._interval);
    };

    // Finalize the task
    that.end = function () {
        that._taskId = 0;
        window.clearTimeout(that._timerId);
    }

    return that;
};

