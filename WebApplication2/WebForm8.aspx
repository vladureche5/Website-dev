<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm8.aspx.cs" Inherits="WebApplication2.WebForm8" %>
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

===================== RESET PASS ======================
<br>
<br>
<input class="srcClass" id="inputMail" type="text" placeholder="Introduceti mail" /><br>
<input class="srcClass" id="inputPass" type="text" placeholder="Introduceti parola noua" /><br>
<input class="srcClass" id="inputPass2" type="text" placeholder="Confirmati parola" /><br>
<br>
<div id="resetBt" class="btnStyle">RESETARE PAROLA</div>
<br>
<br>
<br>
===================================================

    <script>


        let resetB = $("#resetBt");

        resetB.click(function () {    //reset password
            resetpass();

        });

        function resetpass() {
            $("#result").html("");// curata divul
            var form_data = new FormData(); // pregatesti formdata pentru a il trimite in back-end
            form_data.append("front_pass", $("#inputPass").val());
            form_data.append("front_pass2", $("#inputPass2").val());
            form_data.append("front_mail", $("#inputMail").val());

            $.ajax({
                url: 'brain.asmx/resetpassword',// trimiti catre functia select din brain.asmx
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
                        alert(data.message);
                    }
                    console.log(data);
                    
                }

            });
        }













    </script>








</asp:Content>
