<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="WebApplication2.WebForm4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 
    <div style="text-align:center;">===================== LOGIN =====================</div>
    
    <div style="display:grid; justify-items:center; grid-gap:15px; padding-top:20px; padding-bottom:20px;">
        <input class="srcClass" id="inputMail" type="text" placeholder="Introduceti Mail" />
        <input class="srcClass" id="inputPass" type="text" placeholder="Introduceti password" />

        <div class="errMessage" style="color:red;"></div>
  
        <div id="logBt" class="btnStyle">LOG IN</div>
        <a href="webform7">Forgot password?</a>
    </div>

    <div style="text-align:center;">===============================================================</div>
    
  

    <script>

        $(document).ready(function () {

            let logB = $("#logBt");
            let errMessage = $(".errMessage");

            logB.click(function () {    //login
                login();
            });



            //window.location = "default";
            function login() {
                $("#result").html("");// curata divul
                var form_data = new FormData(); // pregatesti formdata pentru a il trimite in back-end
                form_data.append("mail_front", $("#inputMail").val().trim());
                form_data.append("pass_front", $("#inputPass").val().trim());

                $.ajax({
                    url: 'brain.asmx/login',// trimiti catre functia select din brain.asmx
                    type: 'POST',
                    data: form_data,
                    dataType: 'json',
                    contentType: false,
                    cache: false,
                    processData: false,
                    beforeSend: function () { },
                    success: function (data) {
                        if (data.status == "200") {
                            window.location = "WebForm1";
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
