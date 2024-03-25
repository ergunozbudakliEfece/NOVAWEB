


     
    function MachineChange(val, tableId) {
        if (val.value != "SEÇİNİZ") {
            document.getElementById(tableId).getElementsByTagName("tr")[val.parentNode.parentNode.rowIndex].getElementsByTagName("select")[1].removeAttribute("disabled");
            $.getJSON("http://192.168.2.13:83/api/ie/besleme", function (data) {
                var d = data.filter(x => x.HAT_KODU == val.value);


                if (OldKalinlik!= $("#kalinlik").val()) {
                    $(".js-select").select2('destroy');
                    if (document.getElementById(tableId).getElementsByTagName("tr")[val.parentNode.parentNode.rowIndex].getElementsByTagName("select")[2].value == "SEÇİNİZ") {


                        var $sel2 = document.getElementById(tableId).getElementsByTagName("tr")[val.parentNode.parentNode.rowIndex].getElementsByTagName("select")[2];
                        $sel2.replaceChildren();
                        var opt = document.createElement("option");
                        opt.value = "SEÇİNİZ";
                        opt.text = "SEÇİNİZ";
                        opt.setAttribute("selected", "");
                        opt.setAttribute("disabled", "");
                        $sel2.appendChild(opt);
                        for (let i = 0; i < d.length; i++) {
                            var opt1 = document.createElement("option");
                            opt1.value = d[i].HAT_KODU2;
                            opt1.text = d[i].HAT_KODU2;
                            $sel2.appendChild(opt1);
                        }



                    }

                    $(".js-select").select2(
                        {
                            "language": {
                            "noResults": function () {
                            return "Sonuç bulunamadı.";
                        }
                    },
                        escapeMarkup: function (markup) {
                            return markup;
                        }
                        });
                    if (val.value != 'TP01') {
                        DlInputChange(document.getElementById(tableId).getElementsByTagName("tr")[val.parentNode.parentNode.rowIndex].getElementsByTagName("input")[2]);
                    }
                 
                }

                KaydetAktivite();
            }).done(function () {
                if (val.value.substring(0, 2) != "TP") {
                    if (hamkod != null) {
                        if (OldKalinlik != $("#kalinlik").val()) {

                            var url = "http://192.168.2.13:83/api/seri/receteden/" + hamkod + "/0/"+ val.value
                            if (val.value.substring(0, 2) == "BK") {
                                var gen = $('#genislik').val();
                                url = "http://192.168.2.13:83/api/seri/receteden/" + hamkod + "/" + gen + "/" + val.value
                            }
                            $.getJSON(url, function (d1) {

                                var $sel3 = document.getElementById(tableId).getElementsByTagName("tr")[val.parentNode.parentNode.rowIndex].getElementsByTagName("select")[1];
                                $sel3.replaceChildren();
                                var opt2 = document.createElement("option");
                                opt2.value = "SEÇİNİZ";
                                opt2.text = "SEÇİNİZ";
                                opt2.setAttribute("selected", "");
                                opt2.setAttribute("disabled", "");
                                $sel3.appendChild(opt2);
                                for (let i = 0; i < d1.length; i++) {
                                    var opt3 = document.createElement("option");
                                    opt3.value = d1[i].STOK_ADI;
                                    opt3.text = d1[i].STOK_ADI;
                                    $sel3.appendChild(opt3);
                                }
                            })
                            OldKalinlik = null;

                        }
                    }
                } else {


                    var url = "http://192.168.2.13:83/api/seri/receteden/0/0/" + val.value
                    $.getJSON(url, function (d1) {
                        var $sel3 = document.getElementById(tableId).getElementsByTagName("tr")[val.parentNode.parentNode.rowIndex].getElementsByTagName("select")[1];
                        $sel3.replaceChildren();
                        var opt2 = document.createElement("option");
                        opt2.value = "SEÇİNİZ";
                        opt2.text = "SEÇİNİZ";
                        opt2.setAttribute("selected", "");
                        opt2.setAttribute("disabled", "");
                        $sel3.appendChild(opt2);
                        for (let i = 0; i < d1.length; i++) {
                            var opt3 = document.createElement("option");
                            opt3.value = d1[i].MAMUL_KODU;
                            opt3.text = d1[i].STOK_ADI;
                            $sel3.appendChild(opt3);
                        }
                    })

                }


            })
        }
        }
    function RefMachineChange(val) {
        if (val.value != "SEÇİNİZ") {

            document.getElementById("DLTable").getElementsByTagName("tr")[val.parentNode.parentNode.rowIndex].getElementsByTagName("select")[3].removeAttribute("disabled");
            document.getElementById("DLTable").getElementsByTagName("tr")[val.parentNode.parentNode.rowIndex].getElementsByTagName('i')[0].style.display = "block";
            var s = document.getElementById("DLTable").getElementsByTagName("tr")[val.parentNode.parentNode.rowIndex].getElementsByTagName("select")[1].value;



            if (OldKalinlik != $("#kalinlik").val() && s != "SEÇİNİZ") {
                var stokkodu = Stoklar.filter(x => x.STOK_ADI == s)[0].STOK_KODU;
                var url = "http://192.168.2.13:83/api/seri/receteden/" + stokkodu + "/0/" + val.value
                $.getJSON(url, function (d1) {

                    if (d1.length > 0) {
                        var $sel3 = document.getElementById("DLTable").getElementsByTagName("tr")[val.parentNode.parentNode.rowIndex].getElementsByTagName("select")[3];
                        $sel3.replaceChildren();
                        var opt2 = document.createElement("option");
                        opt2.value = "SEÇİNİZ";
                        opt2.text = "SEÇİNİZ";
                        opt2.setAttribute("selected", "");
                        opt2.setAttribute("disabled", "");
                        $sel3.appendChild(opt2);
                        for (let i = 0; i < d1.length; i++) {
                            var opt3 = document.createElement("option");
                            opt3.value = d1[i].STOK_ADI;
                            opt3.text = d1[i].STOK_ADI;
                            $sel3.appendChild(opt3);
                        }
                    }

                })
            }
        } else {

                var $sel3 = document.getElementById("DLTable").getElementsByTagName("tr")[val.parentNode.parentNode.rowIndex].getElementsByTagName("select")[3];
                $sel3.replaceChildren();
                var opt2 = document.createElement("option");
                opt2.value = "SEÇİNİZ";
                opt2.text = "SEÇİNİZ";
                opt2.setAttribute("selected", "");
                opt2.setAttribute("disabled", "");
                $sel3.appendChild(opt2);
            }


        KaydetAktivite();
        }
    function DlInputChange(val) {
        var tr = document.getElementById("DLTable").getElementsByTagName("tr")[val.parentNode.parentNode.rowIndex];
        tr.getElementsByTagName("td")[11].getElementsByTagName("input")[0].value = Math.round(parseFloat((parseFloat(tr.getElementsByTagName("td")[4].getElementsByTagName("input")[0].value.replace(',','.')) * parseFloat(tr.getElementsByTagName("td")[5].getElementsByTagName("input")[0].value)) / ((parseFloat(document.getElementById("genislik").value)) + (parseFloat(document.getElementById("genislikalti").value))) * (parseFloat(document.getElementById("agirlik").value.split('/')[0].split('.')[0] + document.getElementById("agirlik").value.split('/')[0].split('.')[1])) * parseFloat(tr.getElementsByTagName("td")[1].getElementsByTagName("input")[0].value.toString().replace(",", "."))));
            tr.getElementsByTagName("td")[11].getElementsByTagName("input")[1].value = parseFloat(tr.getElementsByTagName("td")[11].getElementsByTagName("input")[0].value)
            var top1 = 0;
            var rows = document.getElementById("DLTable").getElementsByTagName("tr");
            for (let i = 1; i < rows.length; i++) {
                if (rows[i].getElementsByTagName("td")[4].getElementsByTagName("input")[0].value != "") {
                    top1 = top1 + parseFloat(parseFloat(rows[i].getElementsByTagName("td")[4].getElementsByTagName("input")[0].value.replace(',', '.')) * parseFloat(rows[i].getElementsByTagName("td")[5].getElementsByTagName("input")[0].value) * parseFloat(rows[i].getElementsByTagName("input")[0].value));
                }
        }
        var d = parseFloat(((parseFloat(document.getElementById("genislik").value)) + (parseFloat(document.getElementById("genislikalti").value))) - top1);
        document.getElementById("fireagir").value = d.toFixed(2);
            document.getElementById("fireyuzde").value = parseFloat((parseFloat(document.getElementById("fireagir").value) / ((parseFloat(document.getElementById("genislik").value)) + (parseFloat(document.getElementById("genislikalti").value)))) * 100).toFixed(2) + " %";

            document.getElementById("fireagirligi").value = ((parseFloat(document.getElementById("fireagir").value) / ((parseFloat(document.getElementById("genislik").value)) + (parseFloat(document.getElementById("genislikalti").value)))) * (parseFloat(document.getElementById("agirlik").value.split('/')[0].split('.')[0] + document.getElementById("agirlik").value.split('/')[0].split('.')[1]))).toFixed(2);
            KaydetAktivite();
            tr.getElementsByTagName("select")[3].onchange()

        }
    function RefStokChange(val) {
        var tr = document.getElementById("DLTable").getElementsByTagName("tr")[val.parentNode.parentNode.rowIndex];
        axios.get('http://192.168.2.13:83/api/stokkartlari')
            .then(function (response) {
                var jsonObj = response.data;
                var adet = tr.getElementsByTagName("input")[3].value;
                var oo = jsonObj.find(x => x.STOK_ADI == val.value);
                var m = document.getElementById("metraj").value;
                var upt = m.split('/')[0];
                var o = parseFloat(tr.getElementsByTagName("input")[0].value);
                if (oo != undefined) {
                    tr.getElementsByTagName("td")[9].innerHTML = Math.round((parseFloat(upt) / (oo.BOY / 1000)) * o) * adet;
                }

                KaydetAktivite();
                DLCariGetir(val.parentNode.parentNode.rowIndex);
            
            })

    }
     function StokChange(val) {

     KaydetAktivite();
         DLCariGetir(val.parentNode.parentNode.rowIndex);
         if(document.getElementById("DLTable").getElementsByTagName("tr")[val.parentNode.parentNode.rowIndex].getElementsByTagName("select")[2].value!="SEÇİNİZ")
         document.getElementById("DLTable").getElementsByTagName("tr")[val.parentNode.parentNode.rowIndex].getElementsByTagName("select")[2].onchange();
      }
    function oran(val) {
        var tr = document.getElementById("DLTable").getElementsByTagName("tr")[val.parentNode.parentNode.parentNode.rowIndex];
        if (val.value > 1) {
            val.value = "1.00"
        } else if (val.value <= 0) {
            val.value = "0.01"
        }

        DlInputChange(tr.getElementsByTagName("td")[4].getElementsByTagName("input")[0]);
        $(".js-select").trigger("change");
        KaydetAktivite();
    }
    function ParcalioranUst(val) {
        var trust = document.getElementById("DLTable").getElementsByTagName("tr")[val.parentNode.parentNode.parentNode.rowIndex];
        var tralt = document.getElementById("DLTable").getElementsByTagName("tr")[val.parentNode.parentNode.parentNode.rowIndex+1];
        if (val.value > 1) {
            val.value = "1.00"
        } else if (val.value <= 0) {
            val.value = "0.01"
        }
        tralt.getElementsByTagName("input")[0].value = (1 - parseFloat(val.value)).toFixed(2)
        DlInputChange(trust.getElementsByTagName("td")[4].getElementsByTagName("input")[0]);
        DlInputChange(tralt.getElementsByTagName("td")[4].getElementsByTagName("input")[0]);
        $(".js-select").trigger("change");
        KaydetAktivite();
    }
    function ParcalioranAlt(val) {
        var tralt = document.getElementById("DLTable").getElementsByTagName("tr")[val.parentNode.parentNode.rowIndex];
        var trust = document.getElementById("DLTable").getElementsByTagName("tr")[val.parentNode.parentNode.rowIndex-1];
        if (val.value > 1) {
            val.value = "1.00"
        } else if (val.value <= 0) {
            val.value = "0.01"
        }
        trust.getElementsByTagName("input")[0].value = (1 - parseFloat(val.value)).toFixed(2)
        DlInputChange(trust.getElementsByTagName("td")[4].getElementsByTagName("input")[0]);
        DlInputChange(tralt.getElementsByTagName("td")[4].getElementsByTagName("input")[0]);
        $(".js-select").trigger("change");
        KaydetAktivite();
    }
    function addRow() {
        var Tip = $('input[name="uretim-tip"]:checked').val();
        var TableId = "";
        switch (Tip) {
            case 'DL':
                TableId = 'DLTable'
                break;
            case 'TP':
                TableId = 'TPTable'
                break;
        }
        var table = document.getElementById(TableId)
        var totaltr = table.getElementsByTagName("tr");
        var tr = document.createElement("tr");
        $('.js-select').select2("destroy");
        var totaltd = totaltr[1].getElementsByTagName("td").length;
        switch (TableId) {
            case 'DLTable':
                for (let i = 0; i < totaltd; i++) {
                    var td = document.createElement("td");
                    if (i == 6 || i == 10) {

                        td.style = table.getElementsByTagName("tr")[1].getElementsByTagName("td")[i].getAttribute("style");
                    }
                    td.className = table.getElementsByTagName("tr")[1].getElementsByTagName("td")[i].className;
                    td.innerHTML = table.getElementsByTagName("tr")[1].getElementsByTagName("td")[i].innerHTML;
                    if (i == 7) {
                        td.style = "";
                    }
                    tr.appendChild(td);
                    if (i == 9) {
                        td.replaceChildren();
                    }
                    if (i == 0) {
                        td.innerHTML = parseFloat(totaltr[totaltr.length - 1].getElementsByTagName("td")[0].innerHTML) + 1;
                    }
                    if (i == 1) {
                        td.innerHTML = '<div><input class="border-0 text-center" style="font-size:small;max-width:50px" type="number" value="1.00" max="1.00" min="0.00" onblur="oran(this)" /></div><div><img onclick="addRow1(this)" style="width:18px;cursor:pointer" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACQAAAAkCAYAAADhAJiYAAAAAXNSR0IArs4c6QAAAmlJREFUWEftmOFxFDEMhd9VQKiApAJIBUkHQAVABZAKQiogVABUAHRAKiCpINABVEDmu7FuhM5ey3s3zE4G/dm5W9l6lp7l511pYbZaGB7dG0BPJL2QxPOgPEn2taRf5fmpPIeKMJqht5JeFxCZQIC7lHSRccYnC+iZpHeSDrMTB78fks4kfemNzwAiK+dhop9l8m+SCEapMEoI6FNJLOJRGMdck9nqAfpYuGLzAoRJ+T9jL4u/B8bYV63BU4BiZr5KIgC8GDFID4inbhBZYv4tawEi3Z+dNzsGMLtYzPbzGqdagOCFpZnMALBl8OWkvLySBK9aBqktU8Q4io41QL5UcAaiTpXJ+zdLUQJTPoA8KL+3/GuACG4DIF+PwCOAwEHpPxRAxHrosxQBkY3vxYHsZPrOKCCm95Q49h09AqKr0omx95LeJFg8B1AzTgQEIY2g1V1QATgHkN/FbAQ2xtoiIDru4/Lur1SWQQbW42Iym5AF1XZZ3H2eGsQkVhXQHxcpgq0dIYmKrl1qu68aKwZdHKBeyTa1dqnJNMZYSl+yG6entji0OFIvbtv7VFbPmj1t+3RjJN6/PDp+RzncO1xZCT1in4frrQOROlzjWYNkoGu3LNMYbayXH9WzMivQJmVnsjvuJNCIETszq0OOzJGwyA0v8oYlrC06rgpOARRJmzG0D9cnhJnZpBzu3TpqmTKOkTEaKVzw1yCkr12Dop7qKcqhiyJNM96zMlnCB9Boq71cFH1QysXEJnF7gOgzLKR65akNzpSsNo6ODj/sY4NpKA5K+9gA/6yUPeCb93MBpQOMOv4H1MvYHYPjoiWLe0qnAAAAAElFTkSuQmCC"></div>'
                    }
                    if (i == 3 || i == 8) {
                        var HtmlElement = StringToHtml(td.innerHTML);

                        HtmlElement.getElementsByTagName('select')[0].disabled = true;

                        td.innerHTML = HtmlElement.innerHTML;
                    }
                    if ($('#tamami').is(':checked')) {
                        if (i == 7) {
                            var HtmlElement = StringToHtml(td.innerHTML);

                            HtmlElement.getElementsByTagName('select')[0].disabled = false;

                            td.innerHTML = HtmlElement.innerHTML;
                        }
                    } else {
                        if (i == 7) {
                            var HtmlElement = StringToHtml(td.innerHTML);

                            HtmlElement.getElementsByTagName('select')[0].disabled = true;

                            td.innerHTML = HtmlElement.innerHTML;
                        }
                    }
                }
                break;
            case 'TPTable':
                for (let i = 0; i < totaltd; i++) {
                    var td = document.createElement("td");
                    td.style = table.getElementsByTagName("tr")[totaltr.length - 1].getElementsByTagName("td")[i].getAttribute("style");
                    td.className = table.getElementsByTagName("tr")[totaltr.length - 1].getElementsByTagName("td")[i].className;
                    td.innerHTML = table.getElementsByTagName("tr")[totaltr.length - 1].getElementsByTagName("td")[i].innerHTML;
                    tr.appendChild(td);
                    if (i == 8) {
                        td.replaceChildren();
                    }
                    if (i == 0) {
                        td.innerHTML = parseFloat(td.innerHTML) + 1;
                    }
                    if (i == 6) {
                        var HtmlElement = StringToHtml(td.innerHTML);

                        HtmlElement.getElementsByTagName('i')[0].style.display = "none";

                        td.innerHTML = HtmlElement.innerHTML;
                    }
                    if (i == 7) {
                        var HtmlElement = StringToHtml(td.innerHTML);

                        HtmlElement.getElementsByTagName('select')[0].disabled = true;

                        td.innerHTML = HtmlElement.innerHTML;
                    }
                }
                break;

        }
        table.getElementsByTagName("tbody")[0].appendChild(tr);
        $('.js-select').select2(
            {
                "language": {
                    "noResults": function () {
                        return "Sonuç bulunamadı.";
                    }
                },
                escapeMarkup: function (markup) {
                    return markup;
                }
            });;
        KaydetAktivite();
    }
    function addRow1(value) {
        var Tip = $('input[name="uretim-tip"]:checked').val();
        var tableId = "";
        switch (Tip) {
            case 'DL':
                tableId = 'DLTable'
                break;
            case 'BK':
                tableId = 'BKTable'
                break;
        }
    var search = true;
        var rownum = value.parentNode.parentNode.parentNode.parentNode.rowIndex;
        if (rownum == undefined) {
            rownum = value.parentNode.parentNode.parentNode.rowIndex;
        }
        var table = document.getElementById(tableId)
    var totaltr = table.getElementsByTagName("tr");
        var currenttr = table.getElementsByTagName("tr")[rownum];

    var currenttd = currenttr.getElementsByTagName("td");


        if (tableId == 'DLTable') {

            for (let i = 0; i < 4; i++) {
                var val = currenttr.getElementsByTagName("select")[i].value;

                if (val == "SEÇİNİZ") {
                    search = false;
                }
            }

            if (currenttd[4].getElementsByTagName("input")[0].value == null || currenttd[4].getElementsByTagName("input")[0].value == "") {

                search = false
            }

        } else {
            for (let i = 0; i < 3; i++) {
                var val = currenttr.getElementsByTagName("select")[i].value;


                if (val == "SEÇİNİZ") {
                    search = false;
                }
            }

        }

        if (search) {
            currenttr.getElementsByTagName("input")[0].value = "0.5";

            if (tableId == 'DLTable') {
                currenttr.getElementsByTagName("input")[3].value = 1;
                DlInputChange(currenttr.getElementsByTagName("td")[4].getElementsByTagName("input")[0])
                currenttr.getElementsByTagName("select")[3].onchange();
            } else {
                currenttr.getElementsByTagName("td")[1].getElementsByTagName("input")[0].onblur();
            }

            $('.js-select').select2("destroy");
            for (let i = 0; i < currenttd.length; i++) {
                if (tableId == 'DLTable') {
                    if (i == 10 || i == 6) {

                    } else if (i == 7) {
                        currenttd[i].style = "background-color:#edf0f5"
                    }
                    else {

                        currenttd[i].style = "background-color:#edf0f5"
                    }
                } else {
                    currenttd[i].style = "background-color:#edf0f5"
                }

            }
            var totaltd = totaltr[1].getElementsByTagName("td").length;
            var tr = table.insertRow(rownum + 1);
            if (tableId == "DLTable") {
                for (let i = 0; i < totaltd; i++) {
                    var td = tr.insertCell(i);
                    if (i == 10 || i == 6) {
                        td.style = table.getElementsByTagName("tr")[rownum].getElementsByTagName("td")[i].getAttribute("style")
                    } else {
                        td.style = "background-color:#edf0f5"
                    }
                    td.className = table.getElementsByTagName("tr")[rownum].getElementsByTagName("td")[i].className;
                    if (i == 1) {
                        td.innerHTML = '<input class="border-0 text-center" style="font-size:small" type="number" value="0.5" max="1.00" min="0.00" onblur="ParcalioranAlt(this)"><div class="text-center"><img onclick = "removeRow1(this)" style = "width:18px" src = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAAAXNSR0IArs4c6QAAAVJJREFUSEu9leFRwzAMhb9OAkwAnQBGgAlgA+gkwATABMAGMAGwQZkE7uOsnuMmscOl1Z1/xFb09GTpecGObbHj+LQAXAGnwEla5vSZ1gvwOpbkGMA5cAscVliugRUg2JYNAdwB18n7G/D7LWXttmzOgBvgIPnpI1DH+gDy4P7g95gJIlPtPoFu/EsAy/KcTpdZxrVekNFHcrrIy1UCWE8pt2ReggYTYxzFYQ5gtzwA1rx2sUOMIsENixzgEbj8Z/YBGCyeABPuzIG9fQxMqX3JJO7CWMbpAPwk75xV7NUuue+fv73BgxRxVoA5S/QVsrLXS44h6/RxrfjF+Wib6jvHoHXmaO9SIYtc7BwcBWw2sYtAOYhl8/u9kGsfIRMIWdlS0nIOyiy9dAOH3g+xsOYCTXpw8mACuZQBpUSzz50bg/YG7lPTiR3Z5t7y6LdFGvD6BU/pUBk06brSAAAAAElFTkSuQmCC"></div>'
                    } else if (i == 2) {
                        td.innerHTML = '<select class="js-select" style="width:100px" value="DL01" onchange="MachineChange(this, "DLTable")"><option disabled = ""  >SEÇİNİZ</option><option selected="" value="DL01">DL01</option></select>'
                    } else if (i == 3) {
                        td.innerHTML = '<select class="js-select" style="width:350px" value="' + table.getElementsByTagName("tr")[rownum].getElementsByTagName("select")[1].value + '"><option disabled="">SEÇİNİZ</option><option selected="" value="' + table.getElementsByTagName("tr")[rownum].getElementsByTagName("select")[1].value + '" >' + table.getElementsByTagName("tr")[rownum].getElementsByTagName("select")[1].value + '</option></select>'
                    }
                    else if (i == 4) {
                        td.innerHTML = '<input class="border-0 text-center" style="width:80px" readonly oninput="DlInputChange(this)" value="' + table.getElementsByTagName("tr")[rownum].getElementsByTagName("td")[4].getElementsByTagName("input")[0].value + '">'
                    } else if (i == 5) {
                        td.innerHTML = '<input class="border-0 text-center" style="width:80px" readonly oninput="DlInputChange(this)" value="1">'
                    }
                    else if (i == 7) {

                        td.innerHTML = '<i onclick="ReferansSecimiTemizle(this, \'DL\')" data-bs-toggle="tooltip" data-popup="tooltip-custom" data-bs-placement="top" data-bs-html="true" data-bs-original-title="<span>Seçimi Temizle</span>" class="bi bi-x-lg me-2" style="color:#203289;font-size:larger;cursor:pointer;float:left"></i>' + '<select class="js-select" style="width:100px;float: left" value="' + table.getElementsByTagName("tr")[rownum].getElementsByTagName("select")[2].value + '" onchange="RefMachineChange(this)"><option disabled="">SEÇİNİZ</option><option selected="" value="' + table.getElementsByTagName("tr")[rownum].getElementsByTagName("select")[2].value + '" >' + table.getElementsByTagName("tr")[rownum].getElementsByTagName("select")[2].value + '</option></select>'
                    }
                    else if (i == 8) {
                        td.innerHTML = '<select class="js-select" style="width:350px">' + table.getElementsByTagName("tr")[rownum].getElementsByTagName("select")[3].innerHTML + '</select>'
                    }
                    else {
                        td.innerHTML = td.innerHTML = table.getElementsByTagName("tr")[rownum].getElementsByTagName("td")[i].innerHTML;
                    }


                }
            }
            else {
                for (let i = 0; i < totaltd; i++) {
                    var td = tr.insertCell(i);
                    if (i == 10 || i == 6) {
                        td.style = table.getElementsByTagName("tr")[rownum].getElementsByTagName("td")[i].getAttribute("style")
                    } else {
                        td.style = "background-color:#edf0f5"
                    }
                    td.className = table.getElementsByTagName("tr")[rownum].getElementsByTagName("td")[i].className;
                    if (i == 1) {
                        td.innerHTML = '<input class="border-0 text-center" style="font-size:small;width:100px" type="number" value="0.5" max="1.00" min="0.00" onblur="oranBK(this)"><div class="text-center"><img onclick = "removeRow1(this)" style = "width:18px" src = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAAAXNSR0IArs4c6QAAAVJJREFUSEu9leFRwzAMhb9OAkwAnQBGgAlgA+gkwATABMAGMAGwQZkE7uOsnuMmscOl1Z1/xFb09GTpecGObbHj+LQAXAGnwEla5vSZ1gvwOpbkGMA5cAscVliugRUg2JYNAdwB18n7G/D7LWXttmzOgBvgIPnpI1DH+gDy4P7g95gJIlPtPoFu/EsAy/KcTpdZxrVekNFHcrrIy1UCWE8pt2ReggYTYxzFYQ5gtzwA1rx2sUOMIsENixzgEbj8Z/YBGCyeABPuzIG9fQxMqX3JJO7CWMbpAPwk75xV7NUuue+fv73BgxRxVoA5S/QVsrLXS44h6/RxrfjF+Wib6jvHoHXmaO9SIYtc7BwcBWw2sYtAOYhl8/u9kGsfIRMIWdlS0nIOyiy9dAOH3g+xsOYCTXpw8mACuZQBpUSzz50bg/YG7lPTiR3Z5t7y6LdFGvD6BU/pUBk06brSAAAAAElFTkSuQmCC"></div>'
                    }
                    else if (i == 2 ) {
                        table.getElementsByTagName("tr")[rownum].getElementsByTagName("td")[i].getElementsByTagName("select")[0].setAttribute("disabled", "");
                        td.innerHTML = '<select class="js-select" disabled style="width:100px" onchange="MachineChange(this, \'BKTable\')" value="' + table.getElementsByTagName("tr")[rownum].getElementsByTagName("td")[i].getElementsByTagName("select")[0].value +'"><option selected="" value="' + table.getElementsByTagName("tr")[rownum].getElementsByTagName("td")[i].getElementsByTagName("select")[0].value + '" disabled="">' + table.getElementsByTagName("tr")[rownum].getElementsByTagName("td")[i].getElementsByTagName("select")[0].value+'</option></select> ';
                    }
                    else if (i == 4) {
                        td.innerHTML = '<input class="border-0 text-center" style="width:80px" readonly="" value="' + table.getElementsByTagName("tr")[rownum].getElementsByTagName("td")[i].getElementsByTagName("input")[0].value + '">';
                    }
                    else if (i == 5) {
                        td.innerHTML = '<input class="border-0 text-center" style="width:80px;display:none" readonly=""><input class="border-0 text-center" style="width:80px" readonly="" value="' + table.getElementsByTagName("tr")[rownum].getElementsByTagName("td")[i].getElementsByTagName("input")[1].value + '">';
                    }
                    else {
                        td.innerHTML = table.getElementsByTagName("tr")[rownum].getElementsByTagName("td")[i].innerHTML;
                    }


                }
            }
            $('.js-select').select2(
                {
                    "language": {
                        "noResults": function () {
                            return "Sonuç bulunamadı.";
                        }
                    },
                    escapeMarkup: function (markup) {
                        return markup;
                    }
                });
            if (tableId == "DLTable") {
                table.getElementsByTagName("tr")[rownum].getElementsByTagName("input")[0].setAttribute("onblur", "ParcalioranUst(this)")
                ParcalioranAlt(table.getElementsByTagName("tr")[rownum + 1].getElementsByTagName("input")[0])
            }



    }
    else {
        const Toast = Swal.mixin({
            toast: true,
            position: 'center',
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })

        Toast.fire({
            icon: 'info',
            title: 'Lütfen tüm değerleri girdiğinizden emin olun.'
        });
        }
        KaydetAktivite();
    }
    function removeRow() {
        var Tip = $('input[name="uretim-tip"]:checked').val();
        var TableId = "";
        switch (Tip) {
            case 'DL':
                TableId = 'DLTable'
                break;
            case 'TP':
                TableId = 'TPTable'
                break;
        }
        var table = document.getElementById(TableId)
        var totaltr = table.getElementsByTagName("tr");
        if (totaltr.length > 3) {
            if (totaltr[totaltr.length - 1].getElementsByTagName("td")[0].innerHTML == totaltr[totaltr.length - 2].getElementsByTagName("td")[0].innerHTML) {
                totaltr[totaltr.length - 1].remove();
                totaltr[totaltr.length - 1].remove();
            } else {
                totaltr[totaltr.length - 1].remove();
            }

        } else {
            tablosil();
        }
        KaydetAktivite();

    }
    var DLMailTop = null;

    function kaydet() {
        var Tip = $('input[name="uretim-tip"]:checked').val();
        var TableId = "";
        switch (Tip) {
            case "TP":
                TPTableCreatePivotData()
                TPTableCreatePivot2Data()
                break;
            case "RF":
            case "PB":
            case "BK":
                switch (Tip) {
                    case "RF":
                        TableId = "RFTable"
                        break;
                    case "PB":
                        TableId = "PBTable"
                        break;
                    case "BK":
                        TableId = "BKTable"
                        break;
                }
                var l = $(TableId).find("tr").length;
                if (l > 2) {
                    var sum = 0;
                    for (let i = 1; i < l; i++) {
                        sum += parseFloat(parseFloat($(TableId).find("tr").eq(i).find("input").eq(0).val()).toFixed(3));
                    }
                    if (sum != 1) {
                        const Toast = Swal.mixin({
                            toast: true,
                            position: 'center',
                            showConfirmButton: false,
                            timer: 3000,
                            timerProgressBar: true,
                            didOpen: (toast) => {
                                toast.addEventListener('mouseenter', Swal.stopTimer)
                                toast.addEventListener('mouseleave', Swal.resumeTimer)
                            }
                        })

                        Toast.fire({
                            icon: 'warning',
                            title: 'Oranların toplamı 1 olmadığından kaydetme işlemi gerçekleştirilemedi.'
                        });
                    } else {
                        if (Tip == "BK") {

                            BKTableCreatePivotData()
                            BKTableCreatePivot2Data()
                        } else if (Tip == "RF") {
                            RFTableCreatePivotData()
                            RFTableCreatePivot2Data()
                        } else {
                            PBTableCreatePivotData()
                            PBTableCreatePivot2Data()
                        }
                    }
                } else {
                    if (Tip == "BK") {

                        BKTableCreatePivotData()
                        BKTableCreatePivot2Data()
                    }
                 else if (Tip == "RF") {
                    RFTableCreatePivotData()
                    RFTableCreatePivot2Data()
                } else {
                        PBTableCreatePivotData()
                        PBTableCreatePivot2Data()
                       }
                }
                break;
            case "DL":
                var TamamiAktarilsin = $("#tamami").is(":checked");
                var f = parseFloat($("#fireagirligi").val());
                var stok = $("#seri").val();
                if (stok.length > 0) {
                    if (f > 0) {
                        $.getJSON("http://192.168.2.13:83/api/seri/exec/1", function (data) {
                            sonSeri = data;
                        }).done(function () {

                            var jsonObj = @Html.Raw(Json.Encode(ViewBag.SIRANO));
                            var table = document.getElementById("DLTable");
                            var tr = table.getElementsByTagName("tr");

                            if (localStorage.DLlist != null && localStorage.DLlist != "null") {
                                DLList = JSON.parse(localStorage.DLlist)
                                DLListTop = JSON.parse(localStorage.DLlistTop)

                                for (let e = 0; e < DLList.length; e++) {
                                    if (!serilist.includes(DLList[e].GIRDI2)) {
                                        serilist.push(DLList[e].GIRDI2);
                                    }

                                    if (!isemrilist.includes(DLList[e].ISEMRINO)) {
                                        isemrilist.push(DLList[e].ISEMRINO);
                                    }

                                    if (!isemrilist.includes(DLList[e].REF_ISEMRINO)) {
                                        isemrilist.push(DLList[e].REF_ISEMRINO);
                                    }

                                }
                            } else {
                                DLList = [];
                                DLListTop = [];
                            }
                            var Cariler = @Html.Raw(Json.Encode(ViewBag.Cariler));
                            for (let i = 1; i < tr.length; i++) {
                                var adet = tr[i].getElementsByTagName("td")[5].getElementsByTagName("input")[0].value;
                                var oran = tr[i].getElementsByTagName("td")[1].getElementsByTagName("input")[0].value;
                                var tam = document.getElementById("agirlik").value.split("/")[0].split('.').join('');
                                var hamor = document.getElementById("hamoran").value;
                                var sirano = parseFloat(tr[i].getElementsByTagName("td")[0].innerHTML);
                                var girdi2old = sonSeri[0].SERI_NO;
                                var girdi2 = sonSeri[0].SERI_NO;
                                var makine = tr[i].getElementsByTagName("select")[0].value;
                                var makineref = tr[i].getElementsByTagName("select")[2].value;
                                var s = jsonObj.filter(x => x.MAK_KODU == makine);
                                var s1 = jsonObj.filter(x => x.MAK_KODU == makineref);
                                var urunkodu = document.getElementById("seri").value;
                                var tamMi = TamamiAktarilsin ? 'E' : 'H';
                                var stokadi = tr[i].getElementsByTagName("select")[1].value;
                                var kalinlik = document.getElementById("kalinlik").value;
                                var kaplama = document.getElementById("kaplama").value;
                                var kalite = document.getElementById("kalite").value;
                                var siparisno, Sipkont = "";

                                var musteri = tr[i].getElementsByTagName("select")[tr[i].getElementsByTagName("select").length - 1].value;
                                if (musteri != "EFECE GALVANİZ") {
                                    var f = Cariler.filter(x => x.CARI_ISIM == musteri)[0];
                                    siparisno = f.FISNO;
                                    Sipkont = f.SIPKONT;
                                }
                                var genislik = tr[i].getElementsByTagName("td")[4].getElementsByTagName("input")[0].value;
                                var refagirlik = tr[i].getElementsByTagName("td")[11].getElementsByTagName("input")[0].value;

                                var refstokolcusu = tr[i].getElementsByTagName("select")[3].value;
                                var sonrefisemri = "-";
                                for (let a = 0; a < adet; a++) {
                                    var refadet = tr[i].getElementsByTagName("td")[9].innerHTML;
                                    var sonisemri = makine + "24" + (s[s.length - 1].MAX_ISEMRINO).toString().padStart(6, '0') + (a + 1).toString().padStart(3, '0');
                                    if (s1.length > 0) {
                                        sonrefisemri = makineref + "24" + (s1[s1.length - 1].MAX_ISEMRINO).toString().padStart(6, '0') + (a + 1).toString().padStart(3, '0');
                                        if (isemrilist.includes(sonrefisemri)) {
                                            var is1 = isemrilist.filter(x => x.includes(makineref));
                                            sonrefisemri = is1[is1.length - 1].substring(0, 12) + (parseFloat(is1[is1.length - 1].substring(12, 15)) + 1).toString().padStart(3, '0');
                                        }
                                    }


                                    if (isemrilist.includes(sonisemri)) {
                                        var is = isemrilist.filter(x => x.includes(makine));
                                        sonisemri = is[is.length - 1].substring(0, 12) + (parseFloat(is[is.length - 1].substring(12, 15)) + 1).toString().padStart(3, '0');
                                    }

                                    if (serilist.includes(girdi2)) {
                                        //if (refstokolcusu == "SEÇİNİZ") {
                                        //    if (siralist.includes(sirano) && adet == 1) {
                                        //        girdi2 = serilist[serilist.length - 1];
                                        //    } else {
                                        //        girdi2 = girdi2old.substring(0, 12) + (parseFloat(serilist[serilist.length - 1].substring(12, 15)) + 1).toString().padStart(3, '0');
                                        //    }

                                        //}
                                        girdi2 = girdi2old.substring(0, 12) + (parseFloat(serilist[serilist.length - 1].substring(12, 15)) + 1).toString().padStart(3, '0');
                                    }
                                    if (refstokolcusu == "SEÇİNİZ") {
                                        refstokolcusu = "-"
                                        refadet = "-"
                                        if (tamMi=='H') {

                                            girdi2 = "-"
                                        }
                                    } else {
                                        refadet = Math.round(refadet / adet)
                                    }
                                    if (refadet == "-") {
                                        refadet = 0;
                                    }
                                    DLList.push({
                                        GIRDI1: urunkodu, GIRDI2: girdi2, ISEMRINO: sonisemri, STOKADI: stokadi, KALINLIK: kalinlik, KALITE: kalite, KAPLAMA: kaplama, GENISLIK: genislik.replace(',', '.'), ADET: 1, REF_ISEMRINO: sonrefisemri, REF_STOKOLCUSU: refstokolcusu, REF_ADET: refadet, AGIRLIK: Math.round(parseFloat(refagirlik) / adet).toString(), SIPKONT: Sipkont, SIPARISNO: siparisno, HAMORAN: hamor, TAMAGIRLIK: tam, ORAN: oran, "MIKTAR_SABITLE": tamMi
                                    });

                                    if (!(serilist.includes(girdi2)) && (girdi2!="-")) {

                                        serilist.push(girdi2);
                                        say++;

                                    }
                                    if (!siralist.includes(sirano) && girdi2 != "-") {

                                        siralist.push(sirano);
                                    }
                                    isemrilist.push(sonisemri);

                                    if (sonrefisemri != "-") {
                                        isemrilist.push(sonrefisemri);
                                    }


                                }

                                DLListTop.push({ MAKINE: makine, MAKINEREF: makineref, MUSTERI: musteri, STOKADI: (refstokolcusu == "-" ? stokadi : refstokolcusu), KALINLIK: kalinlik, KAPLAMA: kaplama, KALITE: kalite, ADET: adet, AGIRLIK: parseFloat(refagirlik.replace(',', '.')), REF_ADET: (refadet == 0 || refadet == '-' ? adet : (refadet * adet)) });
                                var fark = DLList.length;
                                OldKalinlik = $("#kalinlik").val();
                                siralist = [];
                                var listZ = [];


                                if (localStorage.DLlist != null && localStorage.DLlist != "null") {
                                    var l = JSON.parse(localStorage.DLlist);
                                    for (let z = 0; z < l.length; z++) {

                                        if (!listZ.includes(l[z].GIRDI2)) {
                                            listZ.push(l[z].GIRDI2)
                                        }

                                    }
                                    fark = fark - listZ.length;
                                }
                                var uu = "http://192.168.2.13:83/api/seri/1/" + fark;
                                $.getJSON(uu, function (data) { });
                                localStorage.DLlist = JSON.stringify(DLList);
                                localStorage.DLlistTop = JSON.stringify(DLListTop);

                            }
                            var tpl = $.pivotUtilities.aggregatorTemplates;
                            takiplist = [];
                            if (localStorage.DLMailTop != null && localStorage.DLMailTop != "null") {
                                DLMailTop = JSON.parse(localStorage.DLMailTop)
                                var STOKADLARI = [];
                                var REFSTOKADLARI = [];

                            } else {
                                DLMailTop = [];
                                var STOKADLARI = [];
                                var REFSTOKADLARI = [];



                            }
                            for (let i = 0; i < DLList.length; i++) {
                                if (!STOKADLARI.includes(DLList[i].STOKADI)) {
                                    STOKADLARI.push(DLList[i].STOKADI)
                                }
                                if (!REFSTOKADLARI.includes(DLList[i].REF_STOKOLCUSU)) {
                                    REFSTOKADLARI.push(DLList[i].REF_STOKOLCUSU)
                                }

                            }

                            for (let i = 0; i < STOKADLARI.length; i++) {
                                var filtered = DLList.filter(x => x.STOKADI == STOKADLARI[i]);
                                var sum = 0;
                                for (let j = 0; j < filtered.length; j++) {
                                    sum = sum + parseFloat(filtered[j].AGIRLIK)
                                }
                                var kontrol = DLMailTop.filter(x => x.STOK_ADI == STOKADLARI[i]);
                                if (kontrol.length > 0) {
                                    var index = DLMailTop.findIndex(x => x.STOK_ADI == STOKADLARI[i]);
                                    DLMailTop.splice(index, 1)
                                    DLMailTop.push({
                                        STOK_ADI: STOKADLARI[i], MAKINE: filtered[0].ISEMRINO.substring(0, 4), TOPLAM: sum
                                    })
                                } else {

                                    DLMailTop.push({
                                        STOK_ADI: STOKADLARI[i], MAKINE: filtered[0].ISEMRINO.substring(0, 4), TOPLAM: sum
                                    })
                                }

                            }
                            for (let i = 0; i < REFSTOKADLARI.length; i++) {
                                var filtered = DLList.filter(x => x.REF_STOKOLCUSU == REFSTOKADLARI[i]);
                                var sum = 0;
                                for (let j = 0; j < filtered.length; j++) {
                                    sum = sum + parseFloat(filtered[j].AGIRLIK)
                                }
                                var kontrol = DLMailTop.filter(x => x.STOK_ADI == REFSTOKADLARI[i]);
                                if (kontrol.length > 0) {
                                    var index = DLMailTop.findIndex(x => x.STOK_ADI == REFSTOKADLARI[i]);
                                    DLMailTop.splice(index, 1)
                                    DLMailTop.push({
                                        STOK_ADI: STOKADLARI[i], MAKINE: filtered[0].ISEMRINO.substring(0, 4), TOPLAM: sum
                                    })
                                } else {

                                    DLMailTop.push({
                                        STOK_ADI: STOKADLARI[i], MAKINE: filtered[0].ISEMRINO.substring(0, 4), TOPLAM: sum
                                    })
                                }

                            }
                            localStorage.DLMailTop = JSON.stringify(DLMailTop)
                            function drawPivot() {

                                $(function () {
                                    document.getElementById("DLPivot").replaceChildren();
                                    $("#tamamidiv").show()
                                    $("#tamami").show()
                                    $("#DLPivot").pivot(
                                        takiplist,
                                        {
                                            rows: ["GIRDI1", "GIRDI2", "ISEMRINO", "STOKADI", "KALINLIK", "KALITE", "KAPLAMA", "GENISLIK", "ADET", "REF_ISEMRINO", "REF_STOKOLCUSU", "REF_ADET","MIKTAR_SABITLE", "AGIRLIK", "FIRE", "HAMSARF"],
                                            cols: [],
                                            aggregator: tpl.sum()(["ADET"]),
                                            sorters: {

                                                GIRDI1: function (a, b) { return a - b; } //sort backwards
                                            }


                                        }, false, "tr")
                                })
                            }
                            function getPivot() {

                                document.body.style.cursor = 'default';
                                takiplist = JSON.parse(localStorage.DLlist);
                                var girdi1 = [];
                                for (let i = 0; i < takiplist.length; i++) {

                                    if (!girdi1.includes(takiplist[i].GIRDI1)) {

                                        girdi1.push({ GIRDI1: takiplist[i].GIRDI1 });

                                    }

                                }

                                var newtakip = [];
                                var hamtop = 0;
                                var firtop = 0;
                                for (let i = 0; i < girdi1.length; i++) {
                                    var sum = 0;
                                    var fil = takiplist.filter(x => x.GIRDI1 == girdi1[i].GIRDI1);

                                    for (let e = 0; e < fil.length; e++) {

                                            sum = sum + parseFloat(fil[e].GENISLIK);


                                    }
                                    var ek = 0;

                                    for (let e = 0; e < fil.length; e++) {
                                        if (fil[e].MIKTAR_SABITLE == "E") {
                                            fil[e].HAMSARF = parseFloat(parseFloat(fil[e].GENISLIK) / sum) * (parseFloat(fil[e].TAMAGIRLIK)) * parseFloat(fil[e].ORAN);
                                            var oldek = ek;
                                            ek = ek + fil[e].HAMSARF - parseInt(fil[e].HAMSARF);
                                            if (Math.round(ek) != Math.round(oldek)) {
                                                fil[e].HAMSARF = parseInt(fil[e].HAMSARF) + 1;
                                            } else {
                                                fil[e].HAMSARF = parseInt(fil[e].HAMSARF)
                                            }
                                            fil[e].FIRE = fil[e].HAMSARF - (parseFloat(fil[e].AGIRLIK));
                                            fil[e].AGIRLIK = parseFloat(parseFloat(fil[e].AGIRLIK));

                                        if (fil.length == 1) {
                                            fil[0].FIRE = fil[0].HAMSARF - (parseFloat(fil[0].AGIRLIK));
                                        }
                                    }else {
                                        for (let e = 0; e < fil.length; e++) {
                                            fil[e].HAMSARF = "1";
                                            fil[e].FIRE = "-";
                                            fil[e].AGIRLIK = parseFloat(parseFloat(fil[e].AGIRLIK));
                                        }
                                    }
                                    }


                                    if (newtakip.length == 0) {
                                        newtakip = fil;
                                    } else {
                                        newtakip = newtakip.concat(fil);
                                    }

                                }

                                takiplist = newtakip;
                                var fn = takiplist.filter(x => x.ISEMRINO === takiplist[0].ISEMRINO).length;
                                for (let i = 0; i < takiplist.length; i++) {
                                    tp = tp + parseFloat(takiplist[i].FIRE);
                                }

                                for (let i = 0; i < takiplist.length; i++) {
                                    hamtop = hamtop + takiplist[i].HAMSARF;
                                    firtop = firtop + takiplist[i].FIRE;
                                }
                                //fireag.value = (firtop / fn).toLocaleString();
                                //fireyuz.value = ((firtop / fn) / (hamtop / fn) * 100).toFixed(2) + " %";
                                localStorage.list4 = JSON.stringify(takiplist);
                                drawPivot();




                            }
                            takiplist2 = [];
                            function drawPivot2() {


                                $("#DLPivotTop").pivot(
                                    takiplist2,
                                    {
                                        rows: ["MUSTERI", "STOKADI", "ADET", "AGIRLIK"],




                                    }, false, "tr")
                            }
                            function getPivot2() {

                                var result = Grupla(JSON.parse(localStorage.DLlistTop))
                                for (let i = 0; i < result.length; i++) {

                                    takiplist2.push({ STOKADI: result[i].STOKADI, MUSTERI: result[i].MUSTERI, AGIRLIK: result[i].AGIRLIK, ADET: result[i].ADET })


                                }
                                drawPivot2();
                                document.body.style.cursor = 'default';
                                var rr = document.getElementsByClassName("pvtTable")[0].getElementsByTagName("tr");
                                var sum = 0;
                                var sum2 = 0;
                                for (let i = 1; i < rr.length - 1; i++) {
                                    sum = sum + parseFloat(rr[i].getElementsByTagName("th")[rr[i].getElementsByTagName("th").length - 2].innerHTML);
                                    sum2 = sum2 + parseFloat(rr[i].getElementsByTagName("th")[rr[i].getElementsByTagName("th").length - 1].innerHTML);
                                }

                            }
                            var aggMap = {
                                'agg1': {
                                    aggType: 'Sum',
                                    arguments: ['ADET'],
                                    name: 'ADET TOPLAM',
                                    varName: 'a',
                                    renderEnhancement: 'barchart'
                                },

                                'agg2': {
                                    aggType: 'Sum',
                                    arguments: ['AGIRLIK'],
                                    name: 'AGIRLIK TOPLAM',
                                    varName: 'b',
                                    hidden: false,
                                    renderEnhancement: 'none'
                                },

                            };


                            var customAggs = {};
                            customAggs['Multifact Aggregators'] = $.pivotUtilities.multifactAggregatorGenerator(aggMap, []);

                            var config = {
                                "cols": [],
                                "rows": ["MUSTERI", "STOKOLCULERI", "KALINLIK", "KALITE", "KAPLAMA"],
                                "vals": [],
                                "rowOrder": "key_a_to_z",
                                "colOrder": "key_a_to_z",
                                "aggregatorName": "Multifact Aggregators",
                                "rendererName": "GT Table Heatmap and Barchart",
                                "rendererOptions": {

                                    aggregations: {
                                        defaultAggregations: aggMap,


                                    }
                                }
                            };




                            $.pivotUtilities.customAggs = customAggs;

                            config.aggregators = $.extend($.pivotUtilities.aggregators, $.pivotUtilities.customAggs);

                            config.renderers = $.extend($.pivotUtilities.renderers, $.pivotUtilities.gtRenderers);
                            getPivot();
                            getPivot2();
                            $("#seri").val(null).trigger("input");
                            $("#stokadi").html("")
                            $("#marka").html(null)
                            $("#adet").val(null)
                            $("#kalinlik").val(null)
                            $("#genislik").val(null)
                            $("#genislikalti").val(0)
                            $("#kaplama").val(null)
                            $("#kalite").val(null)
                            $("#metraj").val(null)
                            $("#agirlik").val(null)
                            $("#tamami").hide();

                        })

                    } else {
                        const Toast = Swal.mixin({
                            toast: true,
                            position: 'center',
                            showConfirmButton: false,
                            timer: 3000,
                            timerProgressBar: true,
                            didOpen: (toast) => {
                                toast.addEventListener('mouseenter', Swal.stopTimer)
                                toast.addEventListener('mouseleave', Swal.resumeTimer)
                            }
                        })

                        Toast.fire({
                            icon: 'error',
                            title: 'Fire eksi olamaz!'
                        })

                    }

                } else {
                    const Toast = Swal.mixin({
                        toast: true,
                        position: 'center',
                        showConfirmButton: false,
                        timer: 3000,
                        timerProgressBar: true,
                        didOpen: (toast) => {
                            toast.addEventListener('mouseenter', Swal.stopTimer)
                            toast.addEventListener('mouseleave', Swal.resumeTimer)
                        }
                    })

                    Toast.fire({
                        icon: 'error',
                        title: 'Hammadde seçmediniz!'
                    })
                }
                $('#tamami').show();
                $('#tamamidiv').show();

                break;

        };



    }
    function sil() {
        OldKalinlik = null;
        var Tip = $('input[name="uretim-tip"]:checked').val();
        var TableId = "";
        switch (Tip) {
            case 'DL':
                localStorage.DLlist = null;
                localStorage.DLlistTop = null;
                localStorage.DLMailTop = null;
                $("#tamamidiv").hide();
                document.getElementById("DLPivot").replaceChildren();
                document.getElementById("DLPivotTop").replaceChildren();
                $("#tamamidiv").show();
                $("#tamami").show();
                break;
            case 'TP':
                localStorage.TPPivotVeri = null;
                localStorage.TPPivotToplamVeri = null;
                document.getElementById("TPPivot").replaceChildren();
                document.getElementById("TPPivotTop").replaceChildren();
                $("#tamamidiv").hide();
                break;
            case 'BK':
                localStorage.BKPivotVeri = null;
                localStorage.BKPivotToplamVeri = null;
                document.getElementById("BKPivot").replaceChildren();
                document.getElementById("BKPivotTop").replaceChildren();
                $("#tamamidiv").hide();
                break;
            case 'RF':
                localStorage.RFPivotVeri = null;
                localStorage.RFPivotToplamVeri = null;
                document.getElementById("RFPivot").replaceChildren();
                document.getElementById("RFPivotTop").replaceChildren();
                $("#tamamidiv").hide();
                break;
            case 'PB':
                localStorage.PBPivotVeri = null;
                localStorage.PBPivotToplamVeri = null;
                document.getElementById("PBPivot").replaceChildren();
                document.getElementById("PBPivotTop").replaceChildren();
                $("#tamamidiv").hide();
                break;
        }

    }
    function c(val) {
        if (val.value == "DL") {
            $("#HammaddeCard").hide();
            $("#DLHammaddeCard").show();
            $("#addRw").show();
            $("#removeRw").show();
            $("#DLTable").show();
            $("#DLPivot").show();
            $("#DLPivotTop").show();
            $("#fireTable").show();


            $("#TPTable").hide();
            $("#TPPivot").hide();
            $("#TPPivotTop").hide();

            $("#BKTable").hide();
            $("#BKPivot").hide();
            $("#BKPivotTop").hide();

            $("#PBTable").hide();
            $("#PBPivot").hide();
            $("#PBPivotTop").hide();

            $("#RFTable").hide();
            $("#RFPivot").hide();
            $("#RFPivotTop").hide();
            $("#tamamidiv").show();
            $("#tamami").prop("checked", true);


        }
        else if (val.value == "TP")
        {
            hamkod = null;
            $("#HammaddeCard").hide();
            $("#DLHammaddeCard").hide();
            $("#addRw").show();
            $("#removeRw").show();
            $("#DLTable").hide();
            $("#DLPivot").hide();
            $("#DLPivotTop").hide();
            $("#fireTable").hide();

            $("#TPTable").show();
            $("#TPPivot").show();
            $("#TPPivotTop").show();

            $("#BKTable").hide();
            $("#BKPivot").hide();
            $("#BKPivotTop").hide();

            $("#PBTable").hide();
            $("#PBPivot").hide();
            $("#PBPivotTop").hide();

            $("#RFTable").hide();
            $("#RFPivot").hide();
            $("#RFPivotTop").hide();
            $("#tamamidiv").hide();
            $("#tamami").prop("checked", true);
            TPTableLoadLocalData();
        }
        else if (val.value == "BK") {
            $("#HammaddeCard").show();
            $("#DLHammaddeCard").hide();
            $("#addRw").hide();
            $("#removeRw").hide();
            $("#DLTable").hide();
            $("#DLPivot").hide();
            $("#DLPivotTop").hide();
            $("#fireTable").hide();

            $("#TPTable").hide();
            $("#TPPivot").hide();
            $("#TPPivotTop").hide();

            $("#BKTable").show();
            $("#BKPivot").show();
            $("#BKPivotTop").show();

            $("#PBTable").hide();
            $("#PBPivot").hide();
            $("#PBPivotTop").hide();

            $("#RFTable").hide();
            $("#RFPivot").hide();
            $("#RFPivotTop").hide();
            $("#tamami").prop("checked", true);
            if (localStorage.BKPivotVeri != null && localStorage.BKPivotVeri != undefined && localStorage.BKPivotVeri != "null") {

                $("#tamamidiv").show();
            }
            BKTableLoadLocalData();

        }
        else if (val.value == "PB")
        {
            $("#HammaddeCard").show();
            $("#DLHammaddeCard").hide();
            $("#addRw").hide();
            $("#removeRw").hide();
            $("#DLTable").hide();
            $("#DLPivot").hide();
            $("#DLPivotTop").hide();
            $("#fireTable").hide();

            $("#TPTable").hide();
            $("#TPPivot").hide();
            $("#TPPivotTop").hide();

            $("#BKTable").hide();
            $("#BKPivot").hide();
            $("#BKPivotTop").hide();

            $("#PBTable").show();
            $("#PBPivot").show();
            $("#PBPivotTop").show();

            $("#RFTable").hide();
            $("#RFPivot").hide();
            $("#RFPivotTop").hide();
            $("#tamami").prop("checked", true);
            if (localStorage.PBPivotVeri != null && localStorage.PBPivotVeri != undefined && localStorage.PBPivotVeri != "null") {

                $("#tamamidiv").show();
            } else {
                $("#tamamidiv").hide();
            }
            PBTableLoadLocalData();
        }
        else if (val.value == "RF") {
            $("#HammaddeCard").show();
            $("#DLHammaddeCard").hide();
            $("#addRw").hide();
            $("#removeRw").hide();
            $("#DLTable").hide();
            $("#DLPivot").hide();
            $("#DLPivotTop").hide();
            $("#fireTable").hide();

            $("#TPTable").hide();
            $("#TPPivot").hide();
            $("#TPPivotTop").hide();

            $("#BKTable").hide();
            $("#BKPivot").hide();
            $("#BKPivotTop").hide();

            $("#PBTable").hide();
            $("#PBPivot").hide();
            $("#PBPivotTop").hide();

            $("#RFTable").show();
            $("#RFPivot").show();
            $("#RFPivotTop").show();
            $("#tamami").prop("checked", true);
            if (localStorage.RFPivotVeri != null && localStorage.RFPivotVeri != undefined && localStorage.RFPivotVeri != "null") {

                $("#tamamidiv").show();
            } else {
                $("#tamamidiv").hide();
            }
            RFTableLoadLocalData();
        }
        else
        {
            $("#HammaddeCard").hide();
            $("#DLHammaddeCard").hide();
            $("#addRw").hide();
            $("#removeRw").hide();
            $("#DLTable").hide();
            $("#DLPivot").hide();
            $("#DLPivotTop").hide();
            $("#fireTable").hide();

            $("#TPTable").hide();
            $("#TPPivot").hide();
            $("#TPPivotTop").hide();

            $("#BKTable").hide();
            $("#BKPivot").hide();
            $("#BKPivotTop").hide();

            $("#PBTable").hide();
            $("#PBPivot").hide();
            $("#PBPivotTop").hide();

            $("#RFTable").hide();
            $("#RFPivot").hide();
            $("#RFPivotTop").hide();
            $("#tamamidiv").hide();

        }

        $(".js-select").trigger("change");
        KaydetAktivite();
    }

    function netsisaktar() {

        ShowProgress()
        document.body.style.cursor = 'wait';
        var Tip = $('input[name="uretim-tip"]:checked').val();
        switch (Tip) {
            case 'DL':
                var TamamiAktarilsin = $("#tamami").is(":checked");
                var lis = [];
                lis = takiplist;
                $.ajax({
    url: '@Url.Action("Post","IsEmri")',
    type: 'POST',
                    data: { isemri: lis, TamamiKullanilsin: TamamiAktarilsin },
    success: function () {
        console.log(@TempData["Hata"]);
        HideProgress()

        $("#seri").val(null).trigger("input");
        mail();

        Swal.fire(
            'Veriler başarılı bir şekilde Netsis\'e aktarıldı.',
            '',
            'success'
        ).then((result) => {
            if (result.isConfirmed) {
                location.reload();
            }
        })
        $("#seri").val(null);

        $("#tamamidiv").hide();
        document.getElementById("genislikalti").value = 0;
    },
    error: function () {
        console.log(@TempData["Hata"]);
        document.body.style.cursor = 'default';
        HideProgress()
        const Toast = Swal.mixin({
            toast: true,
            position: 'center',
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })

        Toast.fire({
            icon: 'error',
            title: 'Veriler Netsis\'e aktarılamadı!'
        });
    }
}).done(function () {
                           HideProgress()
                           sil()
                           document.body.style.cursor = 'default';


                                            })
                break;
            case 'TP':
                var lis = [];
                lis = JSON.parse(localStorage.TPPivotVeri);
                                $.ajax({
    url: '@Url.Action("PostTRPZ", "IsEmri")',
    type: 'POST',
    data: { isemri: lis },
    success: function () {

        HideProgress()

        $("#seri").val(null).trigger("input");
        mail();
        const Toast = Swal.mixin({
            toast: true,
            position: 'center',
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })
        var Message =@Html.Raw(Json.Encode(TempData["Hata"]));
        if (Message == "HATA") {
            Toast.fire({
                icon: 'error',
                title: 'Veriler Netsis\'e aktarılamadı.'
            });
        } else {
            Toast.fire({
                icon: 'success',
                title: 'Veriler başarılı bir şekilde Netsis\'e aktarıldı.'
            });
        }

        $("#tamamidiv").hide();
        $("#seri").val(null);
        document.getElementById("genislikalti").value = 0;
    },
    error: function () {
        document.body.style.cursor = 'default';
        HideProgress()
        const Toast = Swal.mixin({
            toast: true,
            position: 'center',
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })

        Toast.fire({
            icon: 'error',
            title: 'Veriler Netsis\'e aktarılamadı!'
        });
    }
}).done(function () {
                           HideProgress()
                           sil()
                           document.body.style.cursor = 'default';


                                            })
                break;
            case 'BK':
            case 'PB':
            case 'RF':
                var lis = [];
                var TamamiAktarilsin = $("#tamami").is(":checked");
                if (Tip == "BK") {
                    lis = JSON.parse(localStorage.BKPivotVeri);
                } else if (Tip == "RF") {
                    lis = JSON.parse(localStorage.RFPivotVeri);
                } else {
                    lis = JSON.parse(localStorage.PBPivotVeri);
                }

                                $.ajax({
                                     url: '@Url.Action("PostBKPBRF", "IsEmri")',
                                    type: 'POST',
                                    data: { isemri: lis, TamamiKullanilsin: TamamiAktarilsin },
    success: function (e) {

        HideProgress()
        console.log(e)

        $("#seri").val(null).trigger("input");
        mail();
        const Toast = Swal.mixin({
            toast: true,
            position: 'center',
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })

        Toast.fire({
            icon: 'success',
            title: 'Veriler başarılı bir şekilde Netsis\'e aktarıldı.'
        });

        $("#tamamidiv").hide();
        $("#seri").val(null);
        document.getElementById("genislikalti").value = 0;
    },
    error: function (e) {
        document.body.style.cursor = 'default';
        HideProgress()
        console.log(e)
        const Toast = Swal.mixin({
            toast: true,
            position: 'center',
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })

        Toast.fire({
            icon: 'error',
            title: 'Veriler Netsis\'e aktarılamadı!'
        });
    }
}).done(function () {
                           HideProgress()
                           sil()
                           document.body.style.cursor = 'default';


                                            })
                break;
        }




    }
    function removeRow1(value) {
        var Tip = $('input[name="uretim-tip"]:checked').val();
        var tableId = "";
        switch (Tip) {
            case 'DL':
                tableId = 'DLTable'
                break;
            case 'BK':
                tableId = 'BKTable'
                break;
        }
        var rownum = value.parentNode.parentNode.parentNode.rowIndex;
        var table = document.getElementById(tableId)
        var currenttr = table.getElementsByTagName("tr")[rownum - 1];
        var currenttd = currenttr.getElementsByTagName("td");
        if (rownum == 2) {
            currenttd[1].getElementsByTagName("img")[0].setAttribute("onclick", "addRow1(this)");
        }
        for (let i = 0; i < currenttd.length; i++) {
            if (rownum == 2) {
                if (i == 6 || i == 10) {

                } else {
                    currenttd[i].removeAttribute("style");
                }


            }
        }
        if (document.getElementById(tableId).getElementsByTagName("tr")[rownum - 1].getElementsByTagName("select")[0].value == "DL01") {
            document.getElementById(tableId).getElementsByTagName("tr")[rownum - 1].getElementsByTagName("input")[3].disabled = false;
        }

        if (rownum == 2) {

            document.getElementById(tableId).getElementsByTagName("tr")[rownum - 1].getElementsByTagName("input")[0].value = "1.00";
        }
        if (tableId == "DLTable") {
            document.getElementById(tableId).getElementsByTagName("tr")[rownum - 1].getElementsByTagName("input")[0].setAttribute("onblur", "oran(this)")
            DlInputChange(document.getElementById(tableId).getElementsByTagName("tr")[rownum - 1].getElementsByTagName("td")[4].getElementsByTagName("input")[0])
            document.getElementById(tableId).getElementsByTagName("tr")[rownum - 1].getElementsByTagName("select")[3].onchange();
        } else {
            document.getElementById(tableId).getElementsByTagName("tr")[rownum - 1].getElementsByTagName("input")[0].setAttribute("onblur", "oranBK(this)")
            if (rownum == 2) {

                document.getElementById(tableId).getElementsByTagName("tr")[rownum - 1].getElementsByTagName("select")[0].removeAttribute("disabled")
                document.getElementById(tableId).getElementsByTagName("tr")[rownum - 1].getElementsByTagName("select")[1].removeAttribute("disabled")
            }
            document.getElementById(tableId).getElementsByTagName("tr")[rownum - 1].getElementsByTagName("input")[0].onblur();
        }

        document.getElementById(tableId).getElementsByTagName("tr")[rownum].remove();
        KaydetAktivite();

    }
    function tablosil() {
        var Tip = $('input[name="uretim-tip"]:checked').val();
        var tableId = `${Tip}Table`;

        switch (Tip) {
            case "TP":
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("input")[0].value = "";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("input")[1].value = "";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("input")[2].value = "";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("input")[3].value = "";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("input")[4].value = "";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("select")[0].value = "SEÇİNİZ";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("select")[1].value = "SEÇİNİZ";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("select")[2].value = "SEÇİNİZ";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("select")[3].value = "SEÇİNİZ";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("select")[3].disabled = true;
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName('i')[0].style.display = "none";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("select")[4].value = "EFECE GALVANİZ";
                break;

            case "DL":
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("input")[3].value = "";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("input")[0].value = "1.00";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("select")[0].value = "SEÇİNİZ";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("select")[1].value = "SEÇİNİZ";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("select")[2].value = "SEÇİNİZ";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("select")[3].value = "SEÇİNİZ";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("select")[4].value = "EFECE GALVANİZ";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("input")[1].value = "";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("input")[2].value = "";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("td")[9].innerHTML = "";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("input")[5].value = "";
                break;

            case "BK":
            case "RF":
            case "PB":
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("input")[0].value = "1.00";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("input")[1].value = "";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("input")[2].value = "";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("input")[3].value = "";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("select")[0].value = "SEÇİNİZ";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("select")[0].removeAttribute("disabled");
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("select")[1].value = "SEÇİNİZ";
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("select")[1].disabled = true;
                document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("select")[2].value = "EFECE GALVANİZ";
                break;

        };

        var rows = document.getElementById(tableId).getElementsByTagName("tr").length;
        var td = document.getElementById(tableId).getElementsByTagName("tr")[1].getElementsByTagName("td")

        for (let i = 0; i < td.length; i++) {
            if (tableId == "DLTable")
            {
                if (i != 6 && i != 10)
                    td[i].removeAttribute("style");
            }
            else if (tableId == "BKTable")
            {
                td[i].removeAttribute("style");
            }
        }

        $(".js-select").select2("destroy").select2(
            {
                "language": {
                    "noResults": function () {
                        return "Sonuç bulunamadı.";
                    }
                },
                escapeMarkup: function (markup) {
                    return markup;
                }
            });;

        if (tableId=="DLTable")
            $("#seri").val(null).trigger("input");

        while (rows > 2)
        {
            document.getElementById(tableId).getElementsByTagName("tr")[rows-1].remove();
            rows--;
        }

        KaydetAktivite();
    }

    //#region Genel Metotlar

    //Referans makinesi seçiminde gerçekleşecek metot. Referans için stok seçimini aktif hale getirir.
    function ReferansMakinesiSecimi(Satir, Tip) {
        if (Satir.value != "SEÇİNİZ") {

            switch (Tip) {
                case "TP":
                    document.getElementById(`${Tip.toUpperCase()}Table`).getElementsByTagName("tr")[Satir.parentNode.parentNode.rowIndex].getElementsByTagName("select")[3].removeAttribute("disabled");
                    document.getElementById(`${Tip.toUpperCase()}Table`).getElementsByTagName("tr")[Satir.parentNode.parentNode.rowIndex].getElementsByTagName('i')[0].style.display = "block";
                    var s = document.getElementById(`${Tip.toUpperCase()}Table`).getElementsByTagName("tr")[Satir.parentNode.parentNode.rowIndex].getElementsByTagName("select")[1].value;

                    var url = "http://192.168.2.13:83/api/seri/receteden/" + s + "/0/" + Satir.value

                    $.getJSON(url, function (d1) {
                        var $sel3 = document.getElementById(`${Tip.toUpperCase()}Table`).getElementsByTagName("tr")[Satir.parentNode.parentNode.rowIndex].getElementsByTagName("select")[3];
                        $sel3.replaceChildren();
                        var opt2 = document.createElement("option");
                        opt2.value = "SEÇİNİZ";
                        opt2.text = "SEÇİNİZ";
                        opt2.setAttribute("selected", "");
                        opt2.setAttribute("disabled", "");
                        $sel3.appendChild(opt2);
                        for (let i = 0; i < d1.length; i++) {
                            var opt3 = document.createElement("option");
                            opt3.value = d1[i].MAMUL_KODU;
                            opt3.text = d1[i].STOK_ADI;
                            $sel3.appendChild(opt3);
                        }
                    })
                    break;
            };

        }

        KaydetAktivite();
    }

    //Referans stok ölçüsü seçildiğinde tablo tipine göre işlem gerçekleştiren metot.
    function ReferansStokSecimi(Satir, Tip)
    {
        switch (Tip) {
            case "TP":
                TPCariGetir(Satir);
                break;
        };

        KaydetAktivite();
    }

    //Referans seçimi temizlendiğinde tablo tipine göre çalışacak metot.
    function ReferansSecimiTemizle(Satir, Tip)
    {
        var SatirNo = Satir.parentNode.parentNode.rowIndex;

        switch (Tip) {
            case "TP":
                document.getElementById(`${Tip.toUpperCase()}Table`).getElementsByTagName("tr")[SatirNo].getElementsByTagName("select")[2].value = "SEÇİNİZ";
                document.getElementById(`${Tip.toUpperCase()}Table`).getElementsByTagName("tr")[SatirNo].getElementsByTagName("select")[3].value = "SEÇİNİZ";
                document.getElementById(`${Tip.toUpperCase()}Table`).getElementsByTagName("tr")[SatirNo].getElementsByTagName("select")[3].disabled = true;

                document.getElementById(`${Tip.toUpperCase()}Table`).getElementsByTagName("tr")[SatirNo].getElementsByTagName('i')[0].style.display = "none";

                TPCariGetir(Satir);
                $('.js-select').select2("destroy");
                $('.js-select').select2(
                    {
                        "language": {
                            "noResults": function () {
                                return "Sonuç bulunamadı.";
                            }
                        },
                        escapeMarkup: function (markup) {
                            return markup;
                        }
                    });;
                break;
            case "DL":
                document.getElementById(`${Tip.toUpperCase()}Table`).getElementsByTagName("tr")[SatirNo].getElementsByTagName("select")[2].value = "SEÇİNİZ";
                document.getElementById(`${Tip.toUpperCase()}Table`).getElementsByTagName("tr")[SatirNo].getElementsByTagName("select")[3].value = "SEÇİNİZ";
                document.getElementById(`${Tip.toUpperCase()}Table`).getElementsByTagName("tr")[SatirNo].getElementsByTagName("select")[3].disabled = true;
                document.getElementById(`${Tip.toUpperCase()}Table`).getElementsByTagName("tr")[SatirNo].getElementsByTagName("td")[9].innerHTML = null;
                document.getElementById(`${Tip.toUpperCase()}Table`).getElementsByTagName("tr")[SatirNo].getElementsByTagName('i')[0].style.display = "none";
                $('.js-select').select2("destroy");
                $('.js-select').select2(
                    {
                        "language": {
                            "noResults": function () {
                                return "Sonuç bulunamadı.";
                            }
                        },
                        escapeMarkup: function (markup) {
                            return markup;
                        }
                    });;
                document.getElementById(`${Tip.toUpperCase()}Table`).getElementsByTagName("tr")[SatirNo].getElementsByTagName("select")[1].onchange();
                break;
        };


        KaydetAktivite();
    }

    //Stok değiştiğinde gerçekleşecek metot.
    function StokDegistir(Satir, Tip) {
        switch (Tip) {
            case "TP":
                TPTableAgirlikHesabi(Satir);
                break;
        };

        TPCariGetir(Satir);
        KaydetAktivite();
    }

    function DLCariGetir(SatirNo)
    {
        var rowNumber = SatirNo;

        var Cariler = @Html.Raw(Json.Encode(ViewBag.Cariler));
        var StokKodu = document.getElementById('DLTable').getElementsByTagName('tr')[rowNumber].getElementsByTagName('select')[3].value;

        if (StokKodu == "SEÇİNİZ") {
            StokKodu = document.getElementById('DLTable').getElementsByTagName('tr')[rowNumber].getElementsByTagName('select')[1].value;
        }

        var FiltreliCariler = Cariler.filter(x => x.STOK_ADI == StokKodu);
        var CariListe = `<option value="EFECE GALVANİZ" disabled selected>EFECE GALVANİZ</option>`;

        for (var i = 0; i < FiltreliCariler.length; i++) {
        CariListe += `<option value="${FiltreliCariler[i].CARI_ISIM}">${FiltreliCariler[i].CARI_ISIM}</option>`;
        }

        document.getElementById('DLTable').getElementsByTagName('tr')[rowNumber].getElementsByTagName('select')[4].innerHTML = CariListe;

    }

    //String olarak gelen veriyi, html element olarak geriye döner.
    function StringToHtml(str) {
        var dom = document.createElement('div');
        dom.innerHTML = str;
        return dom;
    };

    //Kaydet buton aktif/pasif durumunu yöneten metot.
    function KaydetAktivite()
    {
        var Tip = $('input[name="uretim-tip"]:checked').val();
        switch (Tip)
        {
            case "TP":
                KaydetAktiviteGenel("TP");
                break;
            case "DL":
                KaydetAktiviteGenel("DL");
                break;
            case "BK":
                KaydetAktiviteGenel("BK");
                break;
            case "RF":
                KaydetAktiviteGenel("RF");
                break;
            case "PB":
                KaydetAktiviteGenel("PB");
                break;

        };
    }
    function KaydetAktiviteGenel(Tip) {

        var TableId = "";
        switch (Tip) {
            case 'DL':
                TableId = 'DLTable'
                break;
            case 'TP':
                TableId = 'TPTable'
                break;
            case 'BK':
                TableId = 'BKTable'
                break;
            case "RF":
                TableId = 'RFTable'
                break;
            case "PB":
                TableId = 'PBTable'
                break;
        }
        var Satirlar = document.getElementById(TableId).getElementsByTagName('tr');

        var Makine, Stok, Miktar, Adet, RefMakine, RefStok,Seri;
        var AlanlarDolu = true;
        if (TableId != 'DLTable') {
            for (var i = 1; i < Satirlar.length; i++) {
                Makine = document.getElementById(TableId).getElementsByTagName('tr')[i].getElementsByTagName('select')[0].value == "SEÇİNİZ" ? false : true;
                Stok = document.getElementById(TableId).getElementsByTagName('tr')[i].getElementsByTagName('select')[1].value == "SEÇİNİZ" ? false : true;
                var IlkKosul = true;
                var IkinciKosul = false;
                switch (Tip) {
                    case 'TP':
                        Miktar = document.getElementById(TableId).getElementsByTagName('tr')[i].getElementsByTagName('td')[3].getElementsByTagName('input')[0].value.trim().length < 1 ? false : true;
                        Adet = document.getElementById(TableId).getElementsByTagName('tr')[i].getElementsByTagName('td')[4].getElementsByTagName('input')[0].value.trim().length < 1 ? false : true;
                        IlkKosul = Makine && Stok && Miktar && Adet;
                        RefMakine = document.getElementById(TableId).getElementsByTagName('tr')[i].getElementsByTagName('select')[2].value == "SEÇİNİZ" ? false : true;
                        RefStok = document.getElementById(TableId).getElementsByTagName('tr')[i].getElementsByTagName('select')[3].value == "SEÇİNİZ" ? false : true;
                        IkinciKosul = (RefMakine && !RefStok) || (!RefMakine && RefStok);
                        break;
                    case 'DL':
                        Miktar = document.getElementById(TableId).getElementsByTagName('tr')[i].getElementsByTagName('td')[4].getElementsByTagName('input')[0].value.trim().length < 1 ? false : true;
                        Adet = document.getElementById(TableId).getElementsByTagName('tr')[i].getElementsByTagName('td')[5].getElementsByTagName('input')[0].value.trim().length < 1 ? false : true;
                        Seri = document.getElementById("seri").value.trim().length < 1 ? false : true;
                        IlkKosul = Makine && Stok && Miktar && Adet && Seri;
                        RefMakine = document.getElementById(TableId).getElementsByTagName('tr')[i].getElementsByTagName('select')[2].value == "SEÇİNİZ" ? false : true;
                        RefStok = document.getElementById(TableId).getElementsByTagName('tr')[i].getElementsByTagName('select')[3].value == "SEÇİNİZ" ? false : true;
                        IkinciKosul = (RefMakine && !RefStok) || (!RefMakine && RefStok);
                        break;
                    case 'BK':
                    case 'PB':
                    case 'RF':
                        Seri = document.getElementById("seri").value.trim().length < 1 ? false : true;
                        IlkKosul = Makine && Stok && Seri && (parseFloat($("#agirlik").val().split("/")[0].replaceAll(".", "")) != 0);
                        break;
                }

                if (!IlkKosul) {
                    AlanlarDolu = false;
                    break;
                }
                else {
                    if (IkinciKosul) {
                        AlanlarDolu = false;
                        break;
                    }
                }
            }
            $('#kaydet').prop('disabled', !AlanlarDolu);
        }

    }

    function Grupla(Liste) {

        var Grup = [];
        var Tip = $('input[name="uretim-tip"]:checked').val();

        Liste?.forEach(function (a) {

            if (!this[`${a.STOKADI}${a.MUSTERI}`]) {
                this[`${a.STOKADI}${a.MUSTERI}`] = { MUSTERI: a.MUSTERI, STOKADI: a.STOKADI, ADET: 0, AGIRLIK: 0 };
                Grup.push(this[`${a.STOKADI}${a.MUSTERI}`]);
            }
            switch (Tip) {
                case 'DL':
                    this[`${a.STOKADI}${a.MUSTERI}`].ADET += parseFloat(a.REF_ADET);
                    break;
                case 'TP':
                    this[`${a.STOKADI}${a.MUSTERI}`].ADET += parseFloat(a.ADET);
                    break;
                case 'BK':
                case 'PB':
                case 'RF':
                    this[`${a.STOKADI}${a.MUSTERI}`].ADET += parseFloat(a.ADET);
                    break;
            }
            this[`${a.STOKADI}${a.MUSTERI}`].AGIRLIK += a.AGIRLIK;
        }, Object.create(null));

        return Grup;
    }
//#endregion

//#region BK,PB,RF

    function BKCariGetir(SatirNo)
    {

        var rowNumber = SatirNo;

        var Cariler = @Html.Raw(Json.Encode(ViewBag.Cariler));
        var StokKodu = document.getElementById('BKTable').getElementsByTagName('tr')[rowNumber].getElementsByTagName('select')[1].value;

        var FiltreliCariler = Cariler.filter(x => x.STOK_ADI == StokKodu);
        var CariListe = `<option value="EFECE GALVANİZ" disabled selected>EFECE GALVANİZ</option>`;

        for (let i = 0; i < FiltreliCariler.length; i++) {
            CariListe += `<option value="${FiltreliCariler[i].CARI_ISIM}">${FiltreliCariler[i].CARI_ISIM}</option>`;
        }

        document.getElementById('BKTable').getElementsByTagName('tr')[rowNumber].getElementsByTagName('select')[2].innerHTML = CariListe;
    }
    function BKTableLoadLocalData() {
        if (localStorage.BKPivotVeri != null && localStorage.BKPivotVeri != "null") {
            BKTableCreatePivot(JSON.parse(localStorage.BKPivotVeri));
            BKTableCreatePivot2Data();
        }
    }
    function BKTableCreatePivotData()
    {
        var Satirlar = Array.from(document.getElementById('BKTable').getElementsByTagName('tr')).slice(1);
        var SeriNoListe = @Html.Raw(Json.Encode(ViewBag.Seriler));
        axios.get('http://192.168.2.13:83/api/stokkartlari')
            .then(function (response) {
                var StokAdlari = response.data;
                var IsEmirleri = null;

     $.ajax({
         url: `http://192.168.2.13:83/api/isemri/max`,
         type: 'GET',
         success: (data) => {
             IsEmirleri = data;
         }
     }).then(function () {
         var OlusturulanIsEmirleri = [];

         var BKPivotVeri = [];
         var BKPivotToplamVeri = [];

         if (localStorage.BKPivotVeri != null && localStorage.BKPivotVeri != "null") {
             BKPivotVeri = JSON.parse(localStorage.BKPivotVeri);
             BKPivotToplamVeri = JSON.parse(localStorage.BKPivotToplamVeri);

             BKPivotVeri.filter(x => x.ISEMRINO != "-").forEach((item) => { OlusturulanIsEmirleri.push(item.ISEMRINO); });
         }

         var GuncelSeriData;

         $.ajax({
             url: `http://192.168.2.13:83/api/seri/exec/1`,
             type: 'GET',
             success: (data) => {
                 GuncelSeriData = data[0];
             }
         }).then(() => {
             Satirlar.forEach((item) => {
                 var Makine = item.getElementsByTagName('select')[0].value;
                 var Stok = item.getElementsByTagName('select')[1].value;
                 //var Genislik = parseFloat(item.getElementsByTagName('input')[1].value);
                 //var Adet = parseFloat(item.getElementsByTagName('input')[2].value);
                 var Adet2 = parseFloat(item.getElementsByTagName('input')[1].value);
                 var Agirlik = parseFloat(item.getElementsByTagName('input')[3].value);
                 var Girdi1 = $("#seri").val();
                 var Kalinlik = $("#kalinlik").val();
                 var Kalite = $("#kalite").val();
                 var Kaplama = $("#kaplama").val();
                 var Hamoran = $("#hamoran").val();

                 var StokAdi = StokAdlari.filter(x => x.STOK_ADI == Stok).length > 0 ? StokAdlari.filter(x => x.STOK_ADI == Stok)[0].STOK_ADI : "-";

                 var IsEmirleriMakine = IsEmirleri.filter(x => x.MAK_KODU == Makine);
                 var Musteri = item.getElementsByTagName("select")[2].value;

                 var SonIsEmri = Makine + "24" + (IsEmirleriMakine[IsEmirleriMakine.length - 1].MAX_ISEMRINO).toString().padStart(6, '0') + (1).toString().padStart(3, '0');

                 if (OlusturulanIsEmirleri.includes(SonIsEmri)) {
                     SonIsEmri = OlusturulanIsEmirleri[OlusturulanIsEmirleri.length - 1].substring(0, 12) + (parseFloat(OlusturulanIsEmirleri[OlusturulanIsEmirleri.length - 1].substring(12, 15)) + 1).toString().padStart(3, '0');
                 }

                 OlusturulanIsEmirleri.push(SonIsEmri);

                 var Veri = {
                     HAMORAN: Hamoran,
                     ISEMRINO: SonIsEmri,
                     STOKADI: StokAdi,
                     KALINLIK: Kalinlik,
                     KALITE: Kalite,
                     KAPLAMA: Kaplama,
                     GIRDI1: Girdi1,
                     GIRDI2: GuncelSeriData.SERI_NO,
                     ADET2: Adet2,
                     AGIRLIK: Math.round(Agirlik)
                 };

                 var ToplamVeri = {
                     STOKADI: StokAdi,
                     ADET: Adet2,
                     MUSTERI: Musteri,
                     AGIRLIK: Math.round(Agirlik)
                 };
                 OldKalinlik = $("#kalinlik").val();
                 BKPivotVeri.push(Veri);
                 BKPivotToplamVeri.push(ToplamVeri);

             })

             BKTableCreatePivot(BKPivotVeri);

             var fark = BKPivotVeri.length;

             if (localStorage.BKPivotVeri != null && localStorage.BKPivotVeri != "null") {
                 fark = fark - JSON.parse(localStorage.BKPivotVeri).filter(x => x.REF_ISEMRINO != "-").length;
             }

             $.getJSON(`http://192.168.2.13:83/api/seri/1/${fark}`, function (data) { });

             localStorage.setItem('BKPivotVeri', JSON.stringify(BKPivotVeri));
             localStorage.setItem('BKPivotToplamVeri', JSON.stringify(BKPivotToplamVeri));

             BKTableCreatePivot2Data();
             $("#seri").val(null);
             $("#stokadi").html("");
             $("#adet").val(null);
             $("#kalinlik").val(null);
             $("#genislik").val(null);
             $("#kaplama").val(null);
             $("#kalite").val(null);
             $("#metraj").val(null);
             $("#agirlik").val(null);
             $("#tamami").prop('disabled',false);
             $("#tamamidiv").show();
             KaydetAktivite()
         });
     })
            }).catch(function (error) {

            })



    }
    function BKTableCreatePivot2Data() {
        var BKPivotToplamVeri = JSON.parse(localStorage.BKPivotToplamVeri);

        BKTableCreatePivot2(Grupla(BKPivotToplamVeri));
    }
    function BKTableCreatePivot(PivotVeri) {
        $("#BKPivot").pivot(PivotVeri,
            {
                rows: ["GIRDI1", "ISEMRINO", "STOKADI", "KALINLIK", "KALITE", "KAPLAMA", "ADET2", "AGIRLIK"],
                sorters: {
                    ISEMRINO: function (a, b) { return a - b; } //sort backwards
                }
            }, false, "tr");
        $("#BKPivot").find(".pvtAxisLabel").eq(6).text("ADET");
    }
    function BKTableCreatePivot2(PivotToplamVeri) {
        $("#BKPivotTop").pivot(PivotToplamVeri,
            {
                rows: ["MUSTERI", "STOKADI", "ADET", "AGIRLIK"]
            }, false, "tr");
    }
    function StokChangeBK(val) {
        axios.get('http://192.168.2.13:83/api/stokkartlari')
            .then(function (response) {
                var jsonObj = response.data;
                var selectedValue = jsonObj.find(x => x.STOK_ADI == val.value);
                var upt = $("#metraj").val().split('/')[0];
                var agirlik = $("#agirlik").val().split('/')[0].replaceAll(".", "");
                var table = $("#BKTable");
                var rowIndex = $(val).parent().parent().index()

                var currentRow = table.find("tr").eq(rowIndex);
                var makine = table.find("tr").eq(1).find("select").eq(0).val()
                var o = parseFloat(currentRow.find("input").eq(0).val());
                function verileriGetir() {
                    currentRow.find("input").eq(1).val(Math.round((parseFloat(upt) / ((selectedValue.BOY != null ? selectedValue.BOY : 0) / 1000)) * o));
                    currentRow.find("input").eq(3).val(parseFloat(agirlik) * o);
                    BKCariGetir(rowIndex);
                }
                function verileriSifirla() {
                    currentRow.find("input").eq(1).val(null);
                    currentRow.find("input").eq(3).val(null);
                }
                !!makine ? verileriGetir() : verileriSifirla()
                KaydetAktivite();

            })

    }
    function oranBK(val) {
        var table = $("#BKTable");
        var rowIndex =  $(val).parent().parent().index()
        var currentRow = table.find("tr").eq(rowIndex);
        currentRow.find("select").eq(1).trigger("change");
    }


    function PBCariGetir(SatirNo)
    {

        var rowNumber = SatirNo;

        var Cariler = @Html.Raw(Json.Encode(ViewBag.Cariler));
        var StokKodu = document.getElementById('PBTable').getElementsByTagName('tr')[rowNumber].getElementsByTagName('select')[1].value;

        var FiltreliCariler = Cariler.filter(x => x.STOK_ADI == StokKodu);
        var CariListe = `<option value="EFECE GALVANİZ" disabled selected>EFECE GALVANİZ</option>`;

        for (let i = 0; i < FiltreliCariler.length; i++) {
            CariListe += `<option value="${FiltreliCariler[i].CARI_ISIM}">${FiltreliCariler[i].CARI_ISIM}</option>`;
        }

        document.getElementById('PBTable').getElementsByTagName('tr')[rowNumber].getElementsByTagName('select')[2].innerHTML = CariListe;
    }
    function PBTableLoadLocalData() {
        if (localStorage.PBPivotVeri != null && localStorage.PBPivotVeri != "null") {
            PBTableCreatePivot(JSON.parse(localStorage.PBPivotVeri));
            PBTableCreatePivot2Data();
        }
    }
    function PBTableCreatePivotData()
    {
        var Satirlar = Array.from(document.getElementById('PBTable').getElementsByTagName('tr')).slice(1);
        var SeriNoListe = @Html.Raw(Json.Encode(ViewBag.Seriler));
        axios.get('http://192.168.2.13:83/api/stokkartlari')
            .then(function (response) {
                var StokAdlari = response.data;
                var IsEmirleri = null;
                $.ajax({
                    url: `http://192.168.2.13:83/api/isemri/max`,
                    type: 'GET',
                    success: (data) => {
                        IsEmirleri = data;
                    }
                }).then(function () {


                    var OlusturulanIsEmirleri = [];

                    var PBPivotVeri = [];
                    var PBPivotToplamVeri = [];

                    if (localStorage.PBPivotVeri != null && localStorage.PBPivotVeri != "null") {
                        PBPivotVeri = JSON.parse(localStorage.PBPivotVeri);
                        PBPivotToplamVeri = JSON.parse(localStorage.PBPivotToplamVeri);

                        PBPivotVeri.filter(x => x.ISEMRINO != "-").forEach((item) => { OlusturulanIsEmirleri.push(item.ISEMRINO); });
                    }

                    var GuncelSeriData;

                    $.ajax({
                        url: `http://192.168.2.13:83/api/seri/exec/1`,
                        type: 'GET',
                        success: (data) => {
                            GuncelSeriData = data[0];
                        }
                    }).then(() => {
                        Satirlar.forEach((item) => {
                            var Makine = item.getElementsByTagName('select')[0].value;
                            var Stok = item.getElementsByTagName('select')[1].value;
                            //var Genislik = parseFloat(item.getElementsByTagName('input')[1].value);
                            //var Adet = parseFloat(item.getElementsByTagName('input')[1].value);
                            var Adet2 = parseFloat(item.getElementsByTagName('input')[1].value);
                            var Agirlik = parseFloat(item.getElementsByTagName('input')[3].value);
                            var Girdi1 = $("#seri").val();
                            var Kalinlik = $("#kalinlik").val();
                            var Kalite = $("#kalite").val();
                            var Kaplama = $("#kaplama").val();
                            var Hamoran = $("#hamoran").val();

                            var StokAdi = StokAdlari.filter(x => x.STOK_ADI == Stok).length > 0 ? StokAdlari.filter(x => x.STOK_ADI == Stok)[0].STOK_ADI : "-";

                            var IsEmirleriMakine = IsEmirleri.filter(x => x.MAK_KODU == Makine);
                            var Musteri = item.getElementsByTagName("select")[2].value;

                            var SonIsEmri = Makine + "24" + (IsEmirleriMakine[IsEmirleriMakine.length - 1].MAX_ISEMRINO).toString().padStart(6, '0') + (1).toString().padStart(3, '0');

                            if (OlusturulanIsEmirleri.includes(SonIsEmri)) {
                                SonIsEmri = OlusturulanIsEmirleri[OlusturulanIsEmirleri.length - 1].substring(0, 12) + (parseFloat(OlusturulanIsEmirleri[OlusturulanIsEmirleri.length - 1].substring(12, 15)) + 1).toString().padStart(3, '0');
                            }

                            OlusturulanIsEmirleri.push(SonIsEmri);

                            var Veri = {
                                HAMORAN: Hamoran,
                                ISEMRINO: SonIsEmri,
                                STOKADI: StokAdi,
                                KALINLIK: Kalinlik,
                                KALITE: Kalite,
                                KAPLAMA: Kaplama,
                                GIRDI1: Girdi1,
                                GIRDI2: GuncelSeriData.SERI_NO,
                                ADET2: Adet2,
                                AGIRLIK: Math.round(Agirlik)
                            };

                            var ToplamVeri = {
                                STOKADI: StokAdi,
                                ADET: Adet2,
                                MUSTERI: Musteri,
                                AGIRLIK: Math.round(Agirlik)
                            };
                            OldKalinlik = $("#kalinlik").val();
                            PBPivotVeri.push(Veri);
                            PBPivotToplamVeri.push(ToplamVeri);

                        })

                        PBTableCreatePivot(PBPivotVeri);
                        var fark = PBPivotVeri.length;

                        if (localStorage.PBPivotVeri != null && localStorage.PBPivotVeri != "null") {
                            fark = fark - JSON.parse(localStorage.PBPivotVeri).filter(x => x.REF_ISEMRINO != "-").length;
                        }

                        $.getJSON(`http://192.168.2.13:83/api/seri/1/${fark}`, function (data) { });

                        localStorage.setItem('PBPivotVeri', JSON.stringify(PBPivotVeri));
                        localStorage.setItem('PBPivotToplamVeri', JSON.stringify(PBPivotToplamVeri));

                        PBTableCreatePivot2Data();
                        $("#seri").val(null);
                        $("#stokadi").html("");
                        $("#adet").val(null);
                        $("#kalinlik").val(null);
                        $("#genislik").val(null);
                        $("#kaplama").val(null);
                        $("#kalite").val(null);
                        $("#metraj").val(null);
                        $("#agirlik").val(null);
                        KaydetAktivite()
                    });

                })

            })


    }
    function PBTableCreatePivot2Data() {
        var PBPivotToplamVeri = JSON.parse(localStorage.PBPivotToplamVeri);

        PBTableCreatePivot2(Grupla(PBPivotToplamVeri));
    }
    function PBTableCreatePivot(PivotVeri) {
        $("#PBPivot").pivot(PivotVeri,
            {
                rows: ["GIRDI1", "GIRDI2", "ISEMRINO", "STOKADI", "KALINLIK", "KALITE", "KAPLAMA", "ADET2", "AGIRLIK"],
                sorters: {
                    ISEMRINO: function (a, b) { return a - b; } //sort backwards
                }
            }, false, "tr");

    }
    function PBTableCreatePivot2(PivotToplamVeri) {
        $("#PBPivotTop").pivot(PivotToplamVeri,
            {
                rows: ["MUSTERI", "STOKADI", "ADET", "AGIRLIK"]
            }, false, "tr");
    }
    function StokChangePB(val) {
        axios.get('http://192.168.2.13:83/api/stokkartlari')
            .then(function (response) {
                var jsonObj = response.data;
                var selectedValue = jsonObj.find(x => x.STOK_ADI == val.value);
                var upt = $("#metraj").val().split('/')[0];
                var agirlik = $("#agirlik").val().split('/')[0].replaceAll(".", "");
                var table = $("#PBTable");
                var rowIndex = $(val).parent().parent().index()
                var currentRow = table.find("tr").eq(rowIndex);
                var makine = currentRow.find("select").eq(0).val()
                var o = parseFloat(currentRow.find("input").eq(0).val());
                function verileriGetir() {
                    currentRow.find("input").eq(1).val(Math.round((parseFloat(upt) / (selectedValue.BOY / 1000)) * o));
                    currentRow.find("input").eq(3).val(parseFloat(agirlik) * o);
                    PBCariGetir(rowIndex);
                }
                function verileriSifirla() {
                    currentRow.find("input").eq(1).val(null);
                    currentRow.find("input").eq(3).val(null);
                }
                !!makine ? verileriGetir() : verileriSifirla()
                KaydetAktivite();

            })

    }
    function oranPB(val) {
        var table = $("#PBTable");
        var rowIndex = $(val).parent().parent().index()
        var currentRow = table.find("tr").eq(rowIndex);
        currentRow.find("select").eq(1).trigger("change");
    }


    function RFCariGetir(SatirNo)
    {

        var rowNumber = SatirNo;

        var Cariler = @Html.Raw(Json.Encode(ViewBag.Cariler));
        var StokKodu = document.getElementById('RFTable').getElementsByTagName('tr')[rowNumber].getElementsByTagName('select')[1].value;

        var FiltreliCariler = Cariler.filter(x => x.STOK_ADI == StokKodu);
        var CariListe = `<option value="EFECE GALVANİZ" disabled selected>EFECE GALVANİZ</option>`;

        for (let i = 0; i < FiltreliCariler.length; i++) {
            CariListe += `<option value="${FiltreliCariler[i].CARI_ISIM}">${FiltreliCariler[i].CARI_ISIM}</option>`;
        }

        document.getElementById('RFTable').getElementsByTagName('tr')[rowNumber].getElementsByTagName('select')[2].innerHTML = CariListe;
    }
    function RFTableLoadLocalData() {
        if (localStorage.RFPivotVeri != null && localStorage.RFPivotVeri != "null") {
            RFTableCreatePivot(JSON.parse(localStorage.RFPivotVeri));
            RFTableCreatePivot2Data();
        }
    }
    function RFTableCreatePivotData()
    {
        var Satirlar = Array.from(document.getElementById('RFTable').getElementsByTagName('tr')).slice(1);
        var SeriNoListe = @Html.Raw(Json.Encode(ViewBag.Seriler));
        axios.get('http://192.168.2.13:83/api/stokkartlari')
            .then(function (response) {
                var StokAdlari = response.data;
                var IsEmirleri = null;
                $.ajax({
                    url: `http://192.168.2.13:83/api/isemri/max`,
                    type: 'GET',
                    success: (data) => {
                        IsEmirleri = data;
                    }
                }).then(function () {
                    var OlusturulanIsEmirleri = [];

                    var RFPivotVeri = [];
                    var RFPivotToplamVeri = [];

                    if (localStorage.RFPivotVeri != null && localStorage.RFPivotVeri != "null") {
                        RFPivotVeri = JSON.parse(localStorage.RFPivotVeri);
                        RFPivotToplamVeri = JSON.parse(localStorage.RFPivotToplamVeri);

                        RFPivotVeri.filter(x => x.ISEMRINO != "-").forEach((item) => { OlusturulanIsEmirleri.push(item.ISEMRINO); });
                    }

                    var GuncelSeriData;

                    $.ajax({
                        url: `http://192.168.2.13:83/api/seri/exec/1`,
                        type: 'GET',
                        success: (data) => {
                            GuncelSeriData = data[0];
                        }
                    }).then(() => {
                        Satirlar.forEach((item) => {
                            var Makine = item.getElementsByTagName('select')[0].value;
                            var Stok = item.getElementsByTagName('select')[1].value;
                            //var Genislik = parseFloat(item.getElementsByTagName('input')[1].value);
                            //var Adet = parseFloat(item.getElementsByTagName('input')[1].value);
                            var Adet2 = parseFloat(item.getElementsByTagName('input')[1].value);
                            var Agirlik = parseFloat(item.getElementsByTagName('input')[3].value);
                            var Girdi1 = $("#seri").val();
                            var Kalinlik = $("#kalinlik").val();
                            var Kalite = $("#kalite").val();
                            var Kaplama = $("#kaplama").val();
                            var Hamoran = $("#hamoran").val();

                            var StokAdi = StokAdlari.filter(x => x.STOK_ADI == Stok).length > 0 ? StokAdlari.filter(x => x.STOK_ADI == Stok)[0].STOK_ADI : "-";

                            var IsEmirleriMakine = IsEmirleri.filter(x => x.MAK_KODU == Makine);
                            var Musteri = item.getElementsByTagName("select")[2].value;

                            var SonIsEmri = Makine + "24" + (IsEmirleriMakine[IsEmirleriMakine.length - 1].MAX_ISEMRINO).toString().padStart(6, '0') + (1).toString().padStart(3, '0');

                            if (OlusturulanIsEmirleri.includes(SonIsEmri)) {
                                SonIsEmri = OlusturulanIsEmirleri[OlusturulanIsEmirleri.length - 1].substring(0, 12) + (parseFloat(OlusturulanIsEmirleri[OlusturulanIsEmirleri.length - 1].substring(12, 15)) + 1).toString().padStart(3, '0');
                            }

                            OlusturulanIsEmirleri.push(SonIsEmri);

                            var Veri = {
                                HAMORAN: Hamoran,
                                ISEMRINO: SonIsEmri,
                                STOKADI: StokAdi,
                                KALINLIK: Kalinlik,
                                KALITE: Kalite,
                                KAPLAMA: Kaplama,
                                GIRDI1: Girdi1,
                                GIRDI2: GuncelSeriData.SERI_NO,
                                ADET2: Adet2,
                                AGIRLIK: Math.round(Agirlik)
                            };

                            var ToplamVeri = {
                                STOKADI: StokAdi,
                                ADET: Adet2,
                                MUSTERI: Musteri,
                                AGIRLIK: Math.round(Agirlik)
                            };
                            OldKalinlik = $("#kalinlik").val();
                            RFPivotVeri.push(Veri);
                            RFPivotToplamVeri.push(ToplamVeri);

                        })

                        RFTableCreatePivot(RFPivotVeri);
                        var fark = RFPivotVeri.length;

                        if (localStorage.RFPivotVeri != null && localStorage.RFPivotVeri != "null") {
                            fark = fark - JSON.parse(localStorage.RFPivotVeri).filter(x => x.REF_ISEMRINO != "-").length;
                        }

                        $.getJSON(`http://192.168.2.13:83/api/seri/1/${fark}`, function (data) { });

                        localStorage.setItem('RFPivotVeri', JSON.stringify(RFPivotVeri));
                        localStorage.setItem('RFPivotToplamVeri', JSON.stringify(RFPivotToplamVeri));

                        RFTableCreatePivot2Data();

                        $("#seri").val(null);
                        $("#stokadi").html("");
                        $("#adet").val(null);
                        $("#kalinlik").val(null);
                        $("#genislik").val(null);
                        $("#kaplama").val(null);
                        $("#kalite").val(null);
                        $("#metraj").val(null);
                        $("#agirlik").val(null);
                        $("#tamamidiv").hide();
                        KaydetAktivite();
                    });

                })

            })



    }
    function RFTableCreatePivot2Data() {
        var RFPivotToplamVeri = JSON.parse(localStorage.RFPivotToplamVeri);

        RFTableCreatePivot2(Grupla(RFPivotToplamVeri));
    }
    function RFTableCreatePivot(PivotVeri) {
        $("#RFPivot").pivot(PivotVeri,
            {
                rows: ["GIRDI1", "ISEMRINO", "STOKADI", "KALINLIK", "KALITE", "KAPLAMA", "ADET2", "AGIRLIK"],
                sorters: {
                    ISEMRINO: function (a, b) { return a - b; } //sort backwards
                }
            }, false, "tr");
    }
    function RFTableCreatePivot2(PivotToplamVeri) {
        $("#RFPivotTop").pivot(PivotToplamVeri,
            {
                rows: ["MUSTERI", "STOKADI", "ADET", "AGIRLIK"]
            }, false, "tr");
    }
    function StokChangeRF(val) {
        axios.get('http://192.168.2.13:83/api/stokkartlari')
            .then(function (response) {
                var jsonObj = response.data;
                var selectedValue = jsonObj.find(x => x.STOK_ADI == val.value);
                var upt = $("#metraj").val().split('/')[0];
                var agirlik = $("#agirlik").val().split('/')[0].replaceAll(".", "");
                var table = $("#RFTable");
                var rowIndex = $(val).parent().parent().index()
                var currentRow = table.find("tr").eq(rowIndex);
                var makine = currentRow.find("select").eq(0).val()
                var o = parseFloat(currentRow.find("input").eq(0).val());
                function verileriGetir() {
                    currentRow.find("input").eq(1).val(Math.round((parseFloat(upt) / (selectedValue.BOY / 1000)) * o));
                    currentRow.find("input").eq(3).val(parseFloat(agirlik) * o);
                    RFCariGetir(rowIndex);
                }
                function verileriSifirla() {
                    currentRow.find("input").eq(1).val(null);
                    currentRow.find("input").eq(3).val(null);
                }
                !!makine ? verileriGetir() : verileriSifirla()
                KaydetAktivite();

            })

    }
    function oranRF(val) {
        var table = $("#RFTable");
        var rowIndex = $(val).parent().parent().index()
        var currentRow = table.find("tr").eq(rowIndex);
        currentRow.find("select").eq(1).trigger("change");
    }
    function mail() {
        var Tip = $('input[name="uretim-tip"]:checked').val();
        var result;
        switch (Tip) {
            case 'DL':
                result = JSON.parse(localStorage.DLMailTop)
                $.ajax({
                    url: '@Url.Action("Mail","IsEmri")',
                    type: 'POST',
                    data: { mail2: result }
                })
                break;
            case 'TP':
                result = Grupla(JSON.parse(localStorage.TPPivotToplamVeri));

                break;
            case 'BK':
                result = Grupla(JSON.parse(localStorage.BKPivotToplamVeri));

                break;
            case 'MH':
                result = Grupla(JSON.parse(localStorage.MHPivotToplamVeri));

                break;
            case 'RF':
                result = Grupla(JSON.parse(localStorage.RFPivotToplamVeri));

                break;

        }


    }
    function chcked() {
        var b = $('#tamami').is(':checked');
        if (b) {

            $('.selectmakina2').prop('disabled', false);
        } else {
            $('.bi-x-lg ').trigger('click');
            $('.selectmakina2').prop('disabled', true);
        }
    }
   
    function addHammaddeRow() {
        var Tr = $('#DLHammaddeCard').find('table').eq(0).find('tr').eq(1).html();
        $('#DLHammaddeCard').find('table').eq(0).find('tbody').eq(0).append('<tr>'+Tr+'</tr>')
    }
//#endregion
