<%@ Page Title="CRUD" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication2.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .btnStyle {
            display:inline-block;
            padding:5px;
            text-align:center;
            border:1px solid black;
            border-radius:8px;
            max-width:80px;
        }
        .gFilters{
            display:flex;
        }

        .gFilters div{
            cursor:pointer;
        }
    </style>
    

    <div style="display:grid; grid-template-columns:1fr 1fr;">
        <div style="display:grid; grid-gap:10px;">
            <div>Filter inputs</div>
            <div class="gFilters">
                <input class="srcInput" id="srcIdFiltru" type="text" placeholder="ID" />
                <div class="orderBy" direction="ASC" order="id">ASC</div>
            </div>
            <div class="gFilters">
                <input class="srcInput" id="srcNumeFiltru" type="text" placeholder="Nume" />
                <div class="orderBy" direction="ASC" order="nume">ASC</div>
            </div>
            <div class="gFilters">
                <input class="srcInput" id="srcCnpFiltru" type="text" placeholder="CNP" />
                <div class="orderBy" direction="ASC" order="cnp">ASC</div>
            </div>
            <div class="gFilters">
                <input class="srcInput" id="srcMailFiltru" type="text" placeholder="Mail" />
                <div class="orderBy" direction="ASC" order="mail">ASC</div>
            </div>
            <div class="gFilters">
                <input class="srcInput" id="srcTelefonFiltru" type="text" placeholder="Telefon" />
                <div class="orderBy" direction="ASC" order="telefon">ASC</div>
            </div>
            <div id="clickMe2" class="btnStyle">SEARCH</div>
        </div>
        <div>
            <div>
                <div>Insert inputs</div>
                <input class="srcClass" id="srcNume" type="text" placeholder="nume" />
                <input class="srcClass" id="srcCnp" type="text" placeholder="cnp" />
                <input class="srcClass" id="srcMail" type="text" placeholder="mail" />
                <input class="srcClass" id="srcTelefon" type="text" placeholder="telefon" />
                <div id="clickMe" class="btnStyle">INSERT</div>
            </div><div>
                <div>Update inputs</div>
                <input class="srcClassId" type="text" placeholder="id" />
                <input class="srcClassNume"  type="text" placeholder="nume" />
                <input class="srcClassCnp"  type="text" placeholder="cnp" />
                <input class="srcClassMail"  type="text" placeholder="mail" />
                <input class="srcClassTelefon"  type="text" placeholder="telefon" />
                <div id="updateBt"  class="btnStyle">UPDATE</div>
                 <div id="deleteBt"  class="btnStyle">DELETE</div>
            </div>
        </div>
    </div>
    <br /><br /><br />
    <div>View Table:</div>
    <div id="result" style="max-height: 300px;overflow-y: scroll;"> </div>
        


    <script>        

        let srcId = $(".srcClassId");
        let srcIdUpdate = srcId.val();

        let srcNume = $(".srcClassNume");
        let srcNumeUpdate = srcNume.val();

        let srcCnp = $(".srcClassCnp");
        let srcCnpUpdate = srcCnp.val();

        let srcMail = $(".srcClassMail");
        let srcMailUpdate = srcMail.val();

        let srcTelefon = $(".srcClassTelefon");
        let srcTelefonUpdate = srcTelefon.val();

        let result = document.getElementById("result");
        let clickMe = $("#clickMe");  //insert
        let clickMe2 = $("#clickMe2");  //search
        let orderDirection = "ASC";
        let orderElement = "id";
        let updateB = $("#updateBt");
        let deleteB = $("#deleteBt");


   

        
    
        $(".orderBy").click(function () {
           orderElement = $(this).attr("order");
           orderDirection = $(this).attr("direction") == "ASC" ? "DESC" : "ASC";
            
            $(".orderBy").html("ASC");
            $(".orderBy").attr("direction","ASC");
            $(this).html(orderDirection);
            $(this).attr("direction", orderDirection);
            
            fetch2();
        });
        //fetch2();
       
        updateB.click(function () {    //update
            update();
        });

        deleteB.click(async function () {    //delete
            delete2();

        });

        clickMe.click(function () {  //insert
            insert();
        });

        clickMe2.click(function () {    //search
            fetch2();
        });

        $(".srcInput").keyup(function () {
            fetch2();
        });
        

        function insert() {
            $("#result").html("");// curata divul
            var form_data = new FormData(); // pregatesti formdata pentru a il trimite in back-end
            form_data.append("nume_frontend", $("#srcNume").val());
            form_data.append("cnp_frontend", $("#srcCnp").val());
            form_data.append("mail_frontend", $("#srcMail").val());
            form_data.append("telefon_frontend", $("#srcTelefon").val());

            $.ajax({
                url: 'brain.asmx/insert',// trimiti catre functia select din brain.asmx
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
                        result.appendChild(createRow(e));// aici adaugi elementul de mai devreme
                    });
                    fetch2();
                }
            });
        }

        function fetch2() {
            $("#result").html("");// curata divul
            var form_data = new FormData(); // pregatesti formdata pentru a il trimite in back-end

            form_data.append("order_front", orderDirection);
            form_data.append("element_front", orderElement);

            form_data.append("id_frontend_filtru", $("#srcIdFiltru").val());
            form_data.append("nume_frontend_filtru", $("#srcNumeFiltru").val());
            form_data.append("cnp_frontend_filtru", $("#srcCnpFiltru").val());
            form_data.append("mail_frontend_filtru", $("#srcMailFiltru").val());
            form_data.append("telefon_frontend_filtru", $("#srcTelefonFiltru").val());// adauga valoarea ta in form
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
                        result.appendChild(createRow(e));// aici adaugi elementul de mai devreme
                    });
                }
            });
        }


        function update() {
            $("#result").html("");// curata divul
            var form_data = new FormData(); // pregatesti formdata pentru a il trimite in back-end

            form_data.append("id_update", $(".srcClassId").val());
            form_data.append("nume_update", $(".srcClassNume").val());
            form_data.append("cnp_update", $(".srcClassCnp").val());
            form_data.append("mail_update", $(".srcClassMail").val());
            form_data.append("telefon_update", $(".srcClassTelefon").val());
            
            

            $.ajax({
                url: 'brain.asmx/update',// trimiti catre functia select din brain.asmx
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
                        result.appendChild(createRow(e));// aici adaugi elementul de mai devreme
                    });
                    fetch2();
                }
            });
        }

      


        function delete2() {
            $("#result").html("");// curata divul
            var form_data = new FormData(); // pregatesti formdata pentru a il trimite in back-end

            form_data.append("id_delete", $(".srcClassId").val());
            
            $.ajax({
                url: 'brain.asmx/delete',// trimiti catre functia select din brain.asmx
                type: 'POST',
                data: form_data,
                dataType: 'json',
                contentType: false,
                cache: false,
                processData: false,
                beforeSend: function () { },
                success: function (data) {
                    fetch2();
                }
            });
        }

        

        function createRow(e) {
            let info = document.createElement("div");// creezi un element html de tip div
            $(info).click(function () {
                console.log(e.id);
                srcIdUpdate = srcId.val(e.id);
                srcNumeUpdate = srcNume.val(e.nume);
                srcCnpUpdate = srcCnp.val(e.cnp);
                srcMailUpdate = srcMail.val(e.mail);
                srcTelefonUpdate = srcTelefon.val(e.telefon);
            });
            info.style.cssText = `border:1px solid black; border-radius:6px; margin-top:15px; margin-bottom:15px; cursor:pointer;`;
            info.textContent = `${e.id}, ${e.nume}, ${e.cnp}, ${e.mail}, ${e.telefon}  `; // populezi continutul divului cu idul si numele randului
            return info;
        }
       


   
   

    </script>




</asp:Content>
