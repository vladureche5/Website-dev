<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm3.aspx.cs" Inherits="WebApplication2.WebForm3" %>
               
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .btnStyle {
            display: inline-block;
            padding: 5px;
            text-align: center;
            border: 1px solid black;
            border-radius: 8px;
            max-width: 120px;
        }

        .gFilters {
            display: flex;
        }

            .gFilters div {
                cursor: pointer;
            }
    </style>

    <div>Inregistrare Cont</div>
    <input value="hello" class="srcClass" id="inputMail" type="text" placeholder="Introduceti Mail" /><br>
    <input class="srcClass" id="inputPass" type="text" placeholder="Introduceti password" /><br>
    <input class="srcClass" id="inputPass2" type="text" placeholder="Confirmati password" /><br>
    <input class="srcClass" id="inputNick" type="text" placeholder="Introduceti Nickname" /><br>
    <input class="srcClass" id="inputTel" type="text" placeholder="Introduceti Telefon" />

    <br>
    <div id="regBt" class="btnStyle">INREGISTRARE</div>


    <script>
        //alert("Eroare");
        let regB = $("#regBt");

        regB.click(function () {    //inreg
            inreg();

        });

        function inreg() {
            $("#result").html("");// curata divul
            var form_data = new FormData(); // pregatesti formdata pentru a il trimite in back-end
            form_data.append("mail_front", $("#inputMail").val());
            form_data.append("pass_front", $("#inputPass").val());
            form_data.append("pass2_front", $("#inputPass2").val());
            form_data.append("nick_front", $("#inputNick").val());
            form_data.append("tel_front", $("#inputTel").val());
           
            let smth = [1, "1", [], {}];


            $.ajax({
                url: 'brain.asmx/reg',// trimiti catre functia select din brain.asmx
                type: 'POST',
                data: form_data,
                dataType: 'json',
                contentType: false,
                cache: false,
                processData: false,
                beforeSend: function () { },
                success: function (data) {
                    if (parseInt(data.status) == 500) {
                        alert(data.message);
                    }
                    if (parseInt(data.status) == 200) {

                    }
                    console.log(data);
                }

            });
        }


               








    </script>










</asp:Content>
