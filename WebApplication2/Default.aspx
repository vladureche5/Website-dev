<%@ Page Title="Main Menu" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication2._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    
    <!--
        <input class="srcClass" id="src3" type="text" />
        class si id se folosesc pentru a face referinta la element.
        class poate fi comuna si poti sa ai mai multe nume simultan in ea.
        ex: class="cls1 cls2" -> elementul tau paote fi selectat prin cls1,cls2 
                              -> nu ai limita de clase
                              -> mai multe elemente de html pot avea aceeasi clasa
            -> in css si jquery folosesti . (punct) + numele clasei : .srcClass


        id -> in css si jquery folosesti # (diez) + numele idului : #src

        -->
    <div id="result">

    </div>


    <div id="b1" class="box1" style="width:50px; height:50px;"> </div>
    <div id="b2" class="box1" style="width:50px; height:50px;"> </div>



    <script>        

       
        /*
        src.addEventListener("keyup", () => {
            result.textContent = "";
            result.textContent = src.value;
        });
        */



       





















     /*   $("#src").keyup(function () {
            fetch();
        });
        $("#src3").keyup(function () {
            fetch();
        });

        function fetch() {
            $("#result").html("");// curata divul
            var form_data = new FormData(); // pregatesti formdata pentru a il trimite in back-end
            form_data.append("nume_from_front_end", $("#src").val()); // adauga valoarea ta in form
            form_data.append("nume_from_front_end2", $("#src3").val()); // adauga valoarea ta in form
            $.ajax({
                url: 'brain.asmx/select',// trimiti catre functia select din brain.asmx
                type: 'POST',
                data: form_data,
                dataType: 'json',
                contentType: false,
                cache: false,
                processData: false,
                beforeSend: function () { },
                success: function (data) {
                    data.info = JSON.parse(data.info);
                    data.info.map((e, i) => {// long story short asta e un loop / for each bla bla bla
                        let info = document.createElement("div");// creezi un element html de tip div
                        info.textContent = `${e.ID}, ${e.name}  `; // populezi continutul divului cu idul si numele randului
                        result.appendChild(info);// aici adaugi elementul de mai devreme
                    });
                }
            });
        }

        */

       


        //CSS -> .nume -> selectie clasa  (class)
        //    -> #nume -> selectie id      (id)
        /*
            $(document).ready(function () {
                var form_data = new FormData(); // initialize the form
                $.ajax({
                    url: 'brain.asmx/select',
                    type: 'POST',
                    data: form_data,
                    dataType: 'json',
                    contentType: false,
                    cache: false,
                    processData: false,
                    beforeSend: function () { },
                    success: function (data) {
                        data.info = JSON.parse(data.info);
                        //console.log(data.info);
                        data.info.map((e, i) => {
                            console.log(e);
                            let info = document.createElement("div");
                            info.textContent = `${e.ID}, ${e.name}  `;

                            result.appendChild(info);
                        });
                    }
                });
            });
            */
    </script>
</asp:Content>
