$(document).ready(function () {
    $("img").addClass("img-responsive");
});



(function ($, window) {

    $.fn.onEnterKey =
    function (closure) {
        $(this).keypress(
            function (event) {
                var code = event.keyCode ? event.keyCode : event.which;

                if (code == 13) {
                    closure();
                    return false;
                }
            });
    }


    $.fn.contextMenu = function (settings) {

        return this.each(function () {

            // Open context menu
            $(this).on("contextmenu", function (e) {
                //open menu
                $(settings.menuSelector)
                    .data("invokedOn", $(e.target))
                    .show()
                    .css({
                        position: "absolute",
                        left: getLeftLocation(e),
                        top: getTopLocation(e)
                    })
                    .off('click')
                    .on('click', function (e) {
                        $(this).hide();

                        var $invokedOn = $(this).data("invokedOn");
                        var $selectedMenu = $(e.target);

                        settings.menuSelected.call(this, $invokedOn, $selectedMenu);
                    });

                return false;
            });

            //make sure menu closes on any click
            $(document).click(function () {
                $(settings.menuSelector).hide();
            });
        });

        function getLeftLocation(e) {
            var mouseWidth = e.pageX;
            var pageWidth = $(window).width();
            var menuWidth = $(settings.menuSelector).width();

            // opening menu would pass the side of the page
            if (mouseWidth + menuWidth > pageWidth &&
                menuWidth < mouseWidth) {
                return mouseWidth - menuWidth;
            }
            return mouseWidth;
        }

        function getTopLocation(e) {
            var mouseHeight = e.pageY;
            var pageHeight = $(window).height();
            var menuHeight = $(settings.menuSelector).height();

            // opening menu would pass the bottom of the page
            if (mouseHeight + menuHeight > pageHeight &&
                menuHeight < mouseHeight) {
                return mouseHeight - menuHeight;
            }
            return mouseHeight;
        }

    };




    
    window.setInterval(dontgooffline, 60000);

      var dontgooffline= function foo() {
           $.ajax({
               url: '@Url.Action("DontGoOffline", "Home")',
               success: function (data, status) {  },
               error: function (data, status) {  }
           });
       }


    //create hub
       chthub = $.connection.chatHub;
    //start hub
       $.connection.hub.start();

       chthub.client.playCallSound = function (id,name) {
           window.location = "/VideoChat/SingleVideoChat?name=" + name + "&callid=" + id;
       };
       
       chthub.client.callSuccess = function (code) {
           window.location = "/VideoChat/SingleVideoChat?code="+code;
       };

       chthub.client.rcvByClient = function (usrs) {
           var $bread = $('.homeonlineuserdiv');
           $bread.html("");
           $.each(usrs, function (index, value) {
               $(' <a href="#" class="list-group-item homeonlineuser" data-val="' + value.Value.ConnectionId + '" >' + value.Value.ConnectionName + ' </a>')
                 .appendTo($bread);
           });
           $(".homeonlineuser").click(function () {
               window.location = "/VideoChat/Index";
           });
       };



})(jQuery, window);