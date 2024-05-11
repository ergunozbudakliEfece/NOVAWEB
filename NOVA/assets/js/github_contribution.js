/**
 * Jquery plugin to render like contribution graph on Github.
 *
 * @see       {@link https://github.com/bachvtuan/Github-Contribution-Graph}
 * @author    bachvtuan@gmail.com
 * @license   MIT License
 * @since     0.1.0
 */

//Format string
if (!String.prototype.formatString) {
  String.prototype.formatString = function() {
    var args = arguments;
    return this.replace(/{(\d+)}/g, function(match, number) { 
      return typeof args[number] != 'undefined'
        ? args[number]
        : match
      ;
    });
  };
}

(function ( $ ) {


 
    $.fn.github_graph = function( options ) {

        //If the number less than 10, add Zero before it
        var prettyNumber = function( number ){
          return  number < 10 ? '0' + number.toString() : number = number.toString();
        };
 
        /*
        Count the number on each day and store the object
        */
        var processListTimeStamp = function(list_timestamp){

          //The result will store into this varriable
          obj_timestamp = {};
          for (var i=0; i < list_timestamp.length; i++){
            var _type = typeof(list_timestamp[i]);            
            var _d = _type == "number" ? new Date( list_timestamp[i] ) : new Date( list_timestamp[i].timestamp )
            
            var display_date = getDisplayDate( _d );
            var increase = _type == "number" ? 1 : list_timestamp[i].count ;
            if ( !obj_timestamp[ display_date ] ){
              obj_timestamp[ display_date ] = increase;
            }
            else{
              obj_timestamp[ display_date ] += increase;
            }
          }
        }

        var getDisplayDate = function( date_obj ){
          var pretty_month = prettyNumber( date_obj.getMonth() + 1);
          var pretty_date = prettyNumber(date_obj.getDate());
          return "{0}-{1}-{2}".formatString( date_obj.getFullYear(), pretty_month , pretty_date  );
        }
        var getDisplayTime = function (date_obj) {
            var pretty_hour = prettyNumber(date_obj.getHours());
            var pretty_minute = prettyNumber(date_obj.getMinutes());
            return "{0}:{1}".formatString(pretty_hour, pretty_minute);
        }
        var getCount = function( display_date ){
          if ( obj_timestamp[ display_date ] ){
            return obj_timestamp[ display_date];
          }
          return 0;
        }

        var getColor = function(count) {
          if (typeof(settings.colors[0]) == "string"){
            return count > settings.colors.length - 1 ? settings.colors[settings.colors.length-1]: settings.colors[count];
          }

          const isLargeNumber = (element) => element.count > count;
          i = settings.colors.findIndex(isLargeNumber);
          return i == -1 ? settings.colors[settings.colors.length -1].color: settings.colors[ i - 1 ].color;
        }
        
        

        var mouseLeave =function(evt){
          $('.svg-tip').hide();
        }
       
        
        //handle event mouseenter when enter into rect element
        var mouseEnter = function (evt) {
            var target_offset = $(evt.target).offset();
            var count = $(evt.target).attr('data-count');
            var date = $(evt.target).attr('data-date');
            var tarih = date.split("-")[2] + "-" + date.split("-")[1] + "-" + date.split("-")[0];
            var holiday = $(evt.target).attr('data-holiday');
            if (holiday != "") {
                holiday = holiday.split("/");
            }
            var renk = null;
            var url = "http://192.168.2.13:83/api/attendance/find/" + settings.userid
            var u = "";
            if (date != "") {
                if (settings.userData.length == 0) {
                   
                    $.getJSON(url, function (data) {
                        data = data.filter(x => x.DATE === date);

                       
                       
                        for (let i = 0; i < data.length; i++) {

                            if (i == 0) {
                                if (data[i].END_DATE != "0001-01-01T00:00:00" && data[i].END_DATE != null && data[i].END_DATE != "null") {
                                    var text = (holiday[0] != "" ? "</br>" + holiday[0] : "");

                                    if (text == "</br>Ücretsiz İzin" || text == "</br>Ücretli İzin") {
                                        text = text + "</br> Standart Çalışma Saatleri";
                                    }
                                    if (u.includes(text)) {
                                        
                                        text = "";
                                    } if (data[i].START_DATE != "0001-01-01T00:00:00" && data[i].START_DATE != null && data[i].START_DATE != "null") {
                                        u = data[i].DATE.split("-")[2] + "-" + data[i].DATE.split("-")[1] + "-" + data[i].DATE.split("-")[0] + text + "</br>Giriş: " + (new Date(data[i].START_DATE).toLocaleTimeString()).substring(0, 5) + " → Çıkış: " + (new Date(data[i].END_DATE).toLocaleTimeString()).substring(0, 5)
                                    } else {
                                        u = data[i].DATE.split("-")[2] + "-" + data[i].DATE.split("-")[1] + "-" + data[i].DATE.split("-")[0] + text + "</br>Giriş: -  → Çıkış: " + (new Date(data[i].END_DATE).toLocaleTimeString()).substring(0, 5)
                                    }
                                   
                                } else {
                                    u = data[i].DATE.split("-")[2] + "-" + data[i].DATE.split("-")[1] + "-" + data[i].DATE.split("-")[0] + "</br>Giriş: " + (new Date(data[i].START_DATE).toLocaleTimeString()).substring(0, 5) + " → Çıkış: -";
                                }

                            } else {
                                if (data[i].END_DATE != "0001-01-01T00:00:00" && data[i].END_DATE != null && data[i].END_DATE != "null") {
                                    if (data[i].START_DATE != "0001-01-01T00:00:00" && data[i].START_DATE != null && data[i].START_DATE != "null") {
                                        u = u + "</br>Giriş: " + (new Date(data[i].START_DATE).toLocaleTimeString()).substring(0, 5) + " → Çıkış: " + (new Date(data[i].END_DATE).toLocaleTimeString()).substring(0, 5)
                                    } else {
                                        u = u + "</br>Giriş: - → Çıkış: " + (new Date(data[i].END_DATE).toLocaleTimeString()).substring(0, 5)
                                    }
                                   
                                } else {

                                    u = u + "</br>Giriş: " + (new Date(data[i].START_DATE).toLocaleTimeString()).substring(0, 5) + " → Çıkış: -";
                                }
                            }

                        }


                       
                    })
                }
                else {
                    var data1 = settings.userData.filter(x => x.DATE === date);
                   
                    for (let i = 0; i < data1.length; i++) {

                            if (i == 0) {
                                if (data1[i].END_DATE != "0001-01-01T00:00:00" && data1[i].END_DATE != null && data1[i].END_DATE != "null") {
                                    
                                    var text = (holiday[0] != "" && holiday[0] !=null? "</br>" + holiday[0] : "");
                                    
                                    if (text == "</br>Ücretsiz İzin" || text == "</br>Ücretli İzin") {
                                        text = text + "</br> Standart Çalışma Saatleri";
                                    } else if (text == "") {
                                        text = "</br>Çalışılan Saatler";
                                    } 
                                    if (u.includes(text)) {

                                        text = "";
                                    } 
                                    if (data1[i].START_DATE != "0001-01-01T00:00:00" && data1[i].START_DATE != null && data1[i].START_DATE != "null") {
                                        u = data1[i].DATE.split("-")[2] + "-" + data1[i].DATE.split("-")[1] + "-" + data1[i].DATE.split("-")[0] + text + "</br>Giriş: " + (new Date(data1[i].START_DATE).toLocaleTimeString()).substring(0, 5) + " → Çıkış: " + (new Date(data1[i].END_DATE).toLocaleTimeString()).substring(0, 5)

                                    } else {
                                        u = data1[i].DATE.split("-")[2] + "-" + data1[i].DATE.split("-")[1] + "-" + data1[i].DATE.split("-")[0] + text + "</br>Giriş: - → Çıkış: " + (new Date(data1[i].END_DATE).toLocaleTimeString()).substring(0, 5)

                                    }
                                   
                                } else {
                                    
                                    u = data1[i].DATE.split("-")[2] + "-" + data1[i].DATE.split("-")[1] + "-" + data1[i].DATE.split("-")[0] + "</br>Çalışılan Saatler" + "</br>Giriş: " + (new Date(data1[i].START_DATE).toLocaleTimeString()).substring(0, 5) + " → Çıkış: -";
                                }

                            } else {
                               
                                if (data1[i].END_DATE != "0001-01-01T00:00:00" && data1[i].END_DATE != null && data1[i].END_DATE != "null") {
                                    if (data1[i].CODE != 0 && data1[i].CODE != 1) {
                                        
                                        var isim = "</br>"+tumisimler.filter(x => x.DETAIL_CODE == data1[i].CODE)[0].OFFDAY_EXP1;
                                        if (u.includes(isim)) {

                                            isim = "";
                                        }
                                        if (data1[i].START_DATE != "0001-01-01T00:00:00" && data1[i].START_DATE != null && data1[i].START_DATE != "null") {
                                            u = u + isim + "</br>Giriş: " + new Date(data1[i].START_DATE).toLocaleTimeString().substring(0, 5) + " → Çıkış: " + (new Date(data1[i].END_DATE).toLocaleTimeString()).substring(0, 5)
                                        } else {
                                            u = u + isim + "</br>Giriş: - → Çıkış: " + (new Date(data1[i].END_DATE).toLocaleTimeString()).substring(0, 5)
                                        }
                                       
                                    } else {
                                        u = u + "</br>Giriş: " + new Date(data1[i].START_DATE).toLocaleTimeString().substring(0, 5) + " → Çıkış: " + new Date(data1[i].END_DATE).toLocaleTimeString().substring(0, 5)
                                    }
                                   
                                } else {

                                    u = u + "</br>Giriş: " + new Date(data1[i].START_DATE).toLocaleTimeString().substring(0, 5) + " → Çıkış: -";
                                }
                            }

                        }


                       
                   
                }
               
                var count_text = (count >= 1) ? settings.texts[1] + u : settings.texts[0] + " " + tarih + (holiday[0] != "null" && holiday[0] != null ? "</br>" + holiday[0] + ((holiday[1] != null ? "</br>" + holiday[1] : "")) + ("</br>" + (holiday[2] != null ? holiday[2] : "")) : "");
                var text = count_text;

                var svg_tip = $('.svg-tip').show();
                svg_tip.html(text);
                var svg_width = Math.round(svg_tip.width() / 2 + 5);
                var svg_height = svg_tip.height() * 2 + 10;
                if (svg_height == 52.8124) {
                    svg_tip.css({ top: (target_offset.top) - svg_height });
                } else if (svg_height >= 309.688) {
                    svg_tip.css({ top: (target_offset.top) - (svg_height-135) });
                } else {
                    svg_tip.css({ top: (target_offset.top + 40) - svg_height });
                }
                
                svg_tip.css({ left: target_offset.left - svg_width });
            }
            
           
        }
        
        //Append tooltip to display when mouse enter the rect element
        //Default is display:none
        var appendTooltip = function(){
          if ( $('.svg-tip').length == 0 ){
            $('body').append('<div class="svg-tip svg-tip-one-line" style="display:none" ></div>');
          }          
        }
        var data2 = [];
        var settings = $.extend({
          colors: ['#eeeeee','#d6e685','#8cc665','#44a340','#44a340'],
          border:{
            radius: 2,
            hover_color: "#999"
          },
          click: null,
            start_date: null,
            userid: 10000,
            userName: "Personel",
            userData:data2,
          //List of name months
          month_names: ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'],
          h_days : ['M','W','F','S'],
          //Default is empty, it can be overrided
          data:[],
        }, options );

        var _this = $(this);
        var d = null;
        var c = null;
        var tümisimler = null;
        var url = "http://192.168.2.13:83/api/attendance/find/" + settings.userid
        $.getJSON("http://192.168.2.13:83/api/attendance/offday", function (data) {
            tumisimler = data;
        }).done(function () {
            $.getJSON("http://192.168.2.13:83/api/attendance/color", function (data) {
                renk = data;
            }).done(function () {
                $.getJSON(url, function (data) {
                    d = data;
                }).done(function () {
                    $.getJSON("http://192.168.2.13:83/api/attendance/working", function (data) {
                        c = data;
                    }).done(function () {
                        var start = function () {
                            processListTimeStamp(settings.data);
                            var wrap_chart = _this;

                            settings.colors_length = settings.colors.length;
                            var radius = settings.border.radius;
                            var hoverColor = settings.border.hover_color;
                            var clickCallback = settings.click;



                            var start_date;

                            if (settings.start_date == null) {
                                // if set null, will get from 365 days from now
                                start_date = new Date("2023-12-31");
                                start_date.setMonth(start_date.getMonth() - 12);
                                start_date.setDate(start_date.getDate() + 1)

                            } else {
                                // formats:
                                // - YYYY-MM-DD
                                // - YYYY/MM/DD
                                start_date = new Date(settings.start_date);

                            }

                            end_date = new Date(start_date);
                            end_date.setMonth(end_date.getMonth() + 12);
                            end_date.setDate(end_date.getDate() - 1);

                            var loop_html = "";
                            var step = 13;

                            var month_position = [];
                            month_position.push({ month_index: start_date.getMonth(), x: 0 });
                            var using_month = start_date.getMonth();


                            var week = 0;
                            var g_x = week * step;
                            var item_html = '<g transform="translate(' + g_x.toString() + ', 0)">';


                            //Fixed size for now with width= 721 and height = 110
                            for (; start_date.getTime() <= end_date.getTime(); start_date.setDate(start_date.getDate() + 1)) {

                                if ((start_date.getDay() + 6) % 7 == 0) {
                                    var item_html = '<g transform="translate(' + g_x.toString() + ', 0)">';
                                }


                                var month_in_day = start_date.getMonth();
                                var data_date = getDisplayDate(start_date);
                                if ((start_date.getDay() + 6) % 7 == 0 && month_in_day != using_month) {
                                    using_month = month_in_day;
                                    month_position.push({ month_index: using_month, x: g_x });
                                }
                                var count = getCount(data_date);
                                var data1 = d.filter(x => x.DATE === data_date);
                                var sum = 0;
                                if (data1.length > 0) {
                                    if (data1[0].CODE === "0") {
                                        count = 3;
                                    } else if (data1[0].CODE === "1") {
                                        count = 4;
                                    }
                                }



                                var color = null;
                              
                                var kontrol = null;
                                var holi = "";
                                kontrol = c.filter(x => x.DATE === data_date);
                                
                                if (kontrol.length > 0) {

                                    color = renk.filter(x => x.DETAIL_CODE == kontrol[0].CODE)[0].HEX_CODE;

                                    if (kontrol.length > 1) {
                                        for (let i = 0; i < kontrol.length; i++) {
                                            holi += kontrol[i].EXP_1 + "/";
                                        }
                                       
                                    } else {
                                        holi = kontrol[0].EXP_1;
                                    }
                                    


                                } else {
                                    if (data1.length > 0) {
                                       
                                        color = renk.filter(x => x.DETAIL_CODE == data1[0].CODE)[0].HEX_CODE;
                                        if (data1[0].CODE != "1" && data1[0].CODE != "0") {
                                            
                                            holi = (tumisimler.filter(x => x.DETAIL_CODE === parseInt(data1[0].CODE)))[0].OFFDAY_EXP1;
                                            
                                        }
                                        
                                       
                                    } else {
                                        color = getColor(count);
                                    }

                                }
                                var y = (start_date.getDay() + 6) % 7 * step;
                                
                                if (holi != "Haftasonu Tatili" && !holi.includes("Resmi Tatil")) {
                                    var dd = d.filter(x => x.DATE === data_date);
                                    if (dd.length > 0) {
                                        var say = [];
                                        for (let i = 0; i < dd.length; i++) {
                                            say.push(dd[i].CODE)
                                        }
                                        if (say.includes("7") && say.includes("0")) {
                                            
                                            item_html += '<defs><linearGradient id="MyGradient" x2="0%" y2="100%" gradientTransform="rotate(45)"><stop offset="5%" stop-color="#FFFFA8" /><stop offset="5%" stop-color="#D6E685" /></linearGradient></defs>' + '<rect  onclick="openform(this)" class="day isimsiz" width="11" height="11" y="' + y + '" fill="url(#MyGradient)"  data-count="' + count + '" data-date="' + data_date + '" data-holiday="' + holi + '" rx="' + 2 + '" ry="' + 2 + '" data-bs-toggle="popover" data-bs-offset="0,14" data-bs-placement="right" data-bs-html="true" data-bs-content="<p>Hello</p>"/>';
                                           
                                        } else {
                                           
                                            item_html += '<rect onclick="openform(this)" class="day" width="11" height="11" y="' + y + '" fill="' + color + '" data-count="' + count + '" data-date="' + data_date + '" data-holiday="' + holi + '" rx="' + radius + '" ry="' + radius + '" data-bs-toggle="popover" data-bs-offset="0,14" data-bs-placement="right" data-bs-html="true" data-bs-content="<p>Hello</p>"/>';
                                        }


                                    } else {
                                        item_html += '<rect onclick="openform(this)" class="day" width="11" height="11" y="' + y + '" fill="' + color + '" data-count="' + count + '" data-date="' + data_date + '" data-holiday="' + holi + '" rx="' + radius + '" ry="' + radius + '" data-bs-toggle="popover" data-bs-offset="0,14" data-bs-placement="right" data-bs-html="true" data-bs-content="<p>Hello</p>"/>';
                                    }
                                       
                                    } else {
                                        
                                        item_html += '<rect  class="day" width="11" height="11" y="' + y + '" fill="' + color + '" data-count="' + count + '" data-date="' + data_date + '" data-holiday="' + holi + '" rx="' + radius + '" ry="' + radius + '"/>';
                                    }
                                
                               
                                
                                

                                



                                if ((start_date.getDay() + 6) % 7 == 6) {


                                    item_html += "</g>";
                                    loop_html += item_html;

                                    item_html = null;

                                    week++;
                                    g_x = week * step;
                                }



                            }
                            if (item_html != null) {
                                item_html += "</g>";
                                loop_html += item_html;
                            }


                            //trick
                            if (month_position[1].x - month_position[0].x < 40) {
                                //Fix ugly graph by remove first item
                                month_position.shift(0);
                            }
                            var personal_name = settings.userName+" Puantaj Bilgileri";
                            var yy = -70;
                            var xx = -11;
                            var filtrenk = [2,3];
                            if (d.length > 0) {
                                for (let i = 0; i < d.length; i++) {
                                    if (!filtrenk.includes(parseInt(d[i].CODE))) {
                                        filtrenk.push(parseInt(d[i].CODE));
                                    }
                                }
                            }

                            
                                renk = renk.filter(x => filtrenk.includes(x.DETAIL_CODE))

                            
                            
                            for (let i = 0; i < renk.length; i++) {
                                var filt = tumisimler.filter(x => x.DETAIL_CODE == renk[i].DETAIL_CODE);
                                var renkisim = "";
                                if (filt.length > 0) {
                                    renkisim = filt[0].OFFDAY_EXP1;
                                }
                                if (filtrenk.length > 2) {
                                    if (i == 1) {
                                        renkisim = "Tam mesai"
                                    } else if (i == 0) {
                                        renkisim = "Eksik mesai"
                                    }
                                }
                              

                                loop_html += '<rect x="' + xx + '" y="-40" class="day" width="11" height="11"  fill="' + renk[i].HEX_CODE + '"data-date="" rx="' + 2 + '" ry="' + 2 + '" data-bs-toggle="popover" data-bs-offset="0,14" data-bs-placement="right" data-bs-html="true" title="title" data-bs-original-title="Popover Title"/><text style="font-size:6px !important" x= "' + (xx + 15) + '" y="-33" class="month">' + renk[i].EXP_1 + '</text>';
                                xx += 75;






                            }
                            loop_html += '<text style="font-size:12px !important" x="-11" y="-60" class="month">' + personal_name + '</text>'
                            for (var i = 0; i < month_position.length; i++) {
                                var item = month_position[i];
                                var month_name = settings.month_names[item.month_index];
                                if (item.x == 0) {
                                    loop_html += '<text style="font-size:12px !important" x="28" y="-10" class="month">' + month_name + '</text>';
                                } else {
                                    loop_html += '<text style="font-size:12px !important" x="' + item.x + '" y="-10" class="month">' + month_name + '</text>';
                                }

                            }

                            //Add Monday, Wenesday, Friday label
                            loop_html += '<text style="font-size:8px !important" text-anchor="middle" class="wday" dx="-11" dy="9">{0}</text>'.formatString(settings.h_days[0]) +
                                '<text style="font-size:8px !important" text-anchor="middle" class="wday" dx="-11" dy="22">{0}</text>'.formatString(settings.h_days[1]) +
                                '<text style="font-size:8px !important" text-anchor="middle" class="wday" dx="-11" dy="35">{0}</text>'.formatString(settings.h_days[2]) +
                                '<text style="font-size:8px !important" text-anchor="middle" class="wday" dx="-11" dy="48">{0}</text>'.formatString(settings.h_days[3]) +
                                '<text style="font-size:8px !important" text-anchor="middle" class="wday" dx="-11" dy="61">{0}</text>'.formatString(settings.h_days[4]) +
                                '<text style="font-size:8px !important" text-anchor="middle" class="wday" dx="-11" dy="74">{0}</text>'.formatString(settings.h_days[5]) +
                                '<text style="font-size:8px !important" text-anchor="middle" class="wday" dx="-11" dy="87">{0}</text>'.formatString(settings.h_days[6]);
                            var wire_html =
                                '<svg width="721" height="110" viewBox="0 0 721 110"  class="js-calendar-graph-svg">' +
                                '<g transform="translate(20, 20)">' +
                                loop_html +
                                '</g>' +
                                '</svg>';

                            wrap_chart.html(wire_html);

                            $(_this).find(".day").on('click', function () {

                                if (clickCallback) {
                                    clickCallback($(this).attr("data-date"), parseInt($(this).attr("data-count")));
                                }

                            });
                            $(_this).find(".day").hover(function () {
                                $(this).attr("style", "stroke-width: 1; stroke: " + hoverColor);
                            }, function () {
                                $(this).attr("style", "stroke-width:0");
                            });

                            _this.find('rect').on("mouseenter", mouseEnter);
                            _this.find('rect').on("mouseleave", mouseLeave);
                            appendTooltip();



                        }
                        start();
                    })


                })
            })
        })
        
       
        
 
    };
 
}( jQuery ));