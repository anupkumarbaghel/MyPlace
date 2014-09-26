//this is a iifi  instantly invoked function expression , so that there is not global  javascript variable
(function  () {
    //Here we ready our Canvas and Drawing brush(context)
    var canvas = $("#canvasGame");
    var ctx = canvas[0].getContext('2d');
   
    //Preparing  hub for remote movement
    gameHub = $.connection.gameHub;
    $.connection.hub.start();


        //Initilize our variable
        var Fps =10;
        var usr = prompt('Enter your name:', '');
        var colr = get_random_color();
        var _opx = Math.floor((Math.random() * ctx.canvas.width - 100) + 100);
        var _opy = Math.floor((Math.random() * ctx.canvas.height - 100) + 100);

        ///its your player and said p for player and me for you
        var pme = {
            name: usr,
            color: colr,
            opx: _opx,
            opy: _opy,
            x: _opx,
            y: _opy,
            pw:25,
            ph: 25,
            speed:10,
            direction:1
        };


    ///Touch functionality////////////////////////
 
        var initialx = 0; var finalx = 0; var result; var poten = 80;
        var initialy = 0; var finaly = 0;
        document.addEventListener("touchstart", function (e) {
            initialx = e.touches[0].pageX;
            initialy = e.touches[0].pageY;
        });
        document.addEventListener("touchmove", function (e) {
            finalx = e.touches[0].pageX;
            finaly = e.touches[0].pageY;
        });
        document.addEventListener("touchend", function (e) {
            var netobj = {};
            jQuery.extend(netobj, pme);
            if (finalx > initialx+poten)
            {
                netobj.direction = 1;
            }
            else if (finalx < initialx - poten)
            {
                netobj.direction = 3;
            }
           else if (finaly > initialy + poten) {
                netobj.direction = 2;
            }
            else if (finaly < initialy - poten) {
                netobj.direction = 4;
            }
            if (pme.direction != netobj.direction) gameHub.server.send(netobj);
        });
        document.addEventListener("touchcancel", function (e) {
        });
    ///////////////////////////Touch functionality end///////////////////////////////////

    //////////////////Chat feature////////////////////
        gameHub.client.rcvMessage = function (message) {
            addmessage(message);
        };
        $("#btnSend").click(function (e) {
            gameHub.server.sendMessage(pme.name + " : " + document.getElementById("txtWriteMessage").value);
            document.getElementById("txtWriteMessage").value = "";
        });
        function addmessage(tex) {
            var ul = document.getElementById("ulShowMessage");
            var li = document.createElement("li");
            li.appendChild(document.createTextNode(tex));
            ul.appendChild(li);
        }
    //////////////////Chat feature////////////////////



        //Attach the keyboard functionality
        var keysDown = {};
        $('body').bind('keydown', function (e) {
            keysDown[e.which] = true;
        });
        $('body').bind('keyup', function (e) {
            keysDown[e.which] = false;
        });

        var players = [pme];
    //Set the game loop to run at regular interval
        
      var rungame=  setInterval(GameLoop, 1000 / Fps);

    //game loop
        function GameLoop() {
            clearall(players);//clear its own old place
            update(pme);
            moveall(players);
            drawall(players);//it draws only you ,   
           
           

            if (players.length == 2) {
                if (collisionDetection(players[0], players[1])) {
                    clearInterval(rungame);
                    gameHub.server.sendConstant("game over", "dfa", 3);
                    alert("bang bang bang !!!  Game Over!!!!!! refresh to restart");
                    location.reload();
                  
                }
            }
        }

      
        
        gameHub.client.rcvConstant = function (me,ss,a) {
            clearInterval(rungame);
            alert(me);
        };
      
        //Receive message from hub and draw other players 
        gameHub.client.rcvByClient = function (netobj) {
            var isNew = true;
            for (var i = 0; i < players.length;i++)
            {
                var p = players[i];
                if (p.name == netobj.name) {
                    p.direction = netobj.direction;
                        isNew = false;
                } 
            }
            if(isNew)
                players.push(netobj);
        };

   
        //Responsiblility to update the variables by the intruption of keyboad which we attach above or by any means(remotely)
        function update(p) {
            var netobj = {};
            jQuery.extend(netobj, p);
            
            if (keysDown[37]) {
                netobj.direction = 3;
            }
            else if (keysDown[38]) {
                netobj.direction = 4;
            }
            else if (keysDown[39]) {
                netobj.direction = 1;
            }
            else if (keysDown[40]) {
                netobj.direction = 2;
            }
            else if (keysDown[13]) {
                gameHub.server.sendMessage(pme.name + " : " + document.getElementById("txtWriteMessage").value);
                document.getElementById("txtWriteMessage").value = "";
            }

            if (p.direction != netobj.direction) gameHub.server.send(netobj);


        }

      
        var movebox = function (p) {

            switch (p.direction) {
                case 1:
                    p.x += p.speed;
                    break;
                case 2:
                    p.y += p.speed;
                    break;
                case 3:
                    p.x -= p.speed;
                    break;
                case 4:
                    p.y -= p.speed;
                    break;
            }


            p.x = clamp(p.x, 0, ctx.canvas.width - p.pw);
            p.y = clamp(p.y, 0, ctx.canvas.height - p.ph);
        };
        var moveall = function (players) {
            for (var i = 0; i < players.length; i++)
                movebox(players[i]);
        };
        //this function take care object does not move out of the canvas
        function clamp(ppx, min, max) {
            return ppx < min ? min : (ppx > max ? max : ppx);
        }


        //its responsibility to draw only 
        function draw(p) {
            ctx.fillStyle = p.color;
            ctx.fillRect(p.x, p.y, p.pw, p.ph);
            ctx.fillText(p.name, p.x, p.y);
            if (p.name == "anup") console.log(p.x + "," + p.y);
        }
        function drawall(players) {
            for (var i = 0; i < players.length; i++)
                draw(players[i]);
        }

        //it clears the previous drawn shapes or objects
        function clear(p) {
            ctx.clearRect(p.x-20, p.y-20, p.pw*6, p.ph*6);
            //ctx.clearRect(px, py, headWidth, headHeight);
        }
        function clearall(players) {
            for (var i = 0; i < players.length; i++)
                clear(players[i]);
        }




        function get_random_color() {
            function c() {
                return Math.floor(Math.random() * 256).toString(16)
            }
            return "#" + c() + c() + c();
        }

    ///////////////////collision detection/////////////
        function collisionDetection(p1,p2) {
            if (dimension1CollisionDetection(p1.x, p2.x, p1.pw, p2.pw))
                if (dimension1CollisionDetection(p1.y, p2.y, p1.ph, p2.ph))
                    return true;

            return false;
        }
        function dimension1CollisionDetection(x1, x2, xw1, xw2) {
            var mindistance = x1 < x2 ? xw1 : xw2;
            var distance = x1 - x2;
            if (Math.abs(distance) <= mindistance)
                return true;
            return false;
        }
    ////////////////collision detection/////////////////
       

//end of iffi
}());
