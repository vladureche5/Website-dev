<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm7.aspx.cs" Inherits="WebApplication2.WebForm7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div style="text-align:center;">===================== RESET PASS =====================</div>

    <div style="display:grid; justify-items:center; grid-gap:15px; padding-top:20px; padding-bottom:20px;">
        <input class="srcClass" id="inputMail" type="text" placeholder="Introduceti Mail!" />

        <div class="errMessage" style="color:red;"></div>
  
        <div id="mailBt" class="btnStyle">TRIMITE MAIL DE RESETARE</div>
    </div>

    <div style="text-align:center;">===============================================================</div>

  

    <script>
        $(document).ready(function () {

            let mailB = $("#mailBt");

            let errMessage = $(".errMessage");
            mailB.click(function () {    //sendForgotPassword
                sendForgotPassword();
            });

            function sendForgotPassword() {
                $("#result").html("");// curata divul
                var form_data = new FormData(); // pregatesti formdata pentru a il trimite in back-end
                form_data.append("mail_front", $("#inputMail").val());


                $.ajax({
                    url: 'brain.asmx/sendForgotPassword',// trimiti catre functia select din brain.asmx
                    type: 'POST',
                    data: form_data,
                    dataType: 'json',
                    contentType: false,
                    cache: false,
                    processData: false,
                    beforeSend: function () { },
                    success: function (data) {
                        if (data.status == "200") {
                            
                        }
                        if (data.status == "500") {
                            errMessage.html(data.message);
                        }
                    }

                });
            }

        });
    </script>






</asp:Content>
