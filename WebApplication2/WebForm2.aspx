<%@ Page Title="CRYPTED" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="WebApplication2.WebForm2" %>
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
        </style>

    <div>-----------------------------</div>
    <div>Encrypt</div>
    <input class="src" id="inputMail" type="text" placeholder="Mail" />
    <div style="display:flex; grid-gap:10px;">
        <div>Original Text:</div> <input class="srcEnc" type="text" placeholder="password" style="
    border: 0px;
    border-bottom: 1px solid black;
    outline: none;
">
    </div>
    
    <div style="display:flex; grid-gap:10px;">
        <div>Encrypted text:  </div> <div id="result"> </div>
    </div>
    
    <div id="clickMe" class="btnStyle">ENCRYPT</div>
    <div>-----------------------------</div>
    <br />
    <br />
    <div>-----------------------------</div>
    <div>Decrypt</div>
    <input class="src" id="inputMail2" type="text" placeholder="Decrypt KEY" />
        <div style="display:flex; grid-gap:10px;">
        <div>Encrypted Text:</div> <input class="srcDec" type="text" placeholder="Pass to Decrypt" style="
    border: 0px;
    border-bottom: 1px solid black;
    outline: none;
">
    </div>
    <div>Decrypted text: </div>  <div id="result2"> </div>
    <div id="clickMe2" class="btnStyle">DECRYPT</div>
    <div>-----------------------------</div>

   

    <script>

        let res = document.getElementById("result");
        let res2 = document.getElementById("result2");
        let encB = $("#clickMe");
        let decB = $("#clickMe2");

        encB.click(function () {    //encrypt
            encrypt();
        });

        decB.click(function () {    //decrypt
            decrypt();
        });

    function encrypt() {
       // $("#result").html("");// curata divul
        var form_data = new FormData(); // pregatesti formdata pentru a il trimite in back-end     

        form_data.append("pass_front", $(".srcEnc").val());
        form_data.append("inputMail", $("#inputMail").val());


        $.ajax({
            url: 'brain.asmx/crypt',// trimiti catre functia select din brain.asmx
            type: 'POST',
            dataType: 'json',
            data: form_data,
            contentType: false,
            cache: false,
            processData: false,
            beforeSend: function () { },
            success: function (data) {
                console.log(data);
                

                

                    let info = document.createElement("div");
                    info.textContent = `${data.info}`;
                    res.appendChild(info);
                
                
            }
        });

        }
        


        function decrypt() {
           // $("#result").html("");// curata divul
            var form_data = new FormData(); // pregatesti formdata pentru a il trimite in back-end     

           
            form_data.append("inputPass", $(".srcDec").val());
            form_data.append("inputMail", $("#inputMail2").val());

            $.ajax({
                url: 'brain.asmx/decrypt',// trimiti catre functia select din brain.asmx
                type: 'POST',
                dataType: 'json',
                data: form_data,
                contentType: false,
                cache: false,
                processData: false,
                beforeSend: function () { },
                success: function (data) {
                    console.log(data);
                    let info = document.createElement("div");
                    info.textContent = `${data.info}`;
                    res2.appendChild(info);
                    }
                
            });

        }

   /*     function insert() {
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
        */ 

    </script>



  

</asp:Content>

