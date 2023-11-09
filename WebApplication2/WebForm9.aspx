<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm9.aspx.cs" Inherits="WebApplication2.WebForm9" %>
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
        <div>Filtered Search for Cars</div>
        <div class="gFilters">
            <input class="srcInput" id="srcIdFiltru" type="text" placeholder="ID" />
            <div class="orderBy" direction="ASC" order="id">ASC</div>
        </div>
        <div class="gFilters">
            <input class="srcInput" id="srcMarcaFiltru" type="text" placeholder="Marca" />
            <div class="orderBy" direction="ASC" order="nume">ASC</div>
        </div>
        <div class="gFilters">
            <input class="srcInput" id="srcModelFiltru" type="text" placeholder="Model" />
            <div class="orderBy" direction="ASC" order="cnp">ASC</div>
        </div>
        <div id="searchB" class="btnStyle">SEARCH</div>
  
        
    
    </div>
    <div>
        <div>
            <div>Insert Inputs for Cars</div>
            <input class="srcClass" id="srcFkUser" type="text" placeholder="fk_user" />
            <input class="srcClass" id="srcFiles" multiple type="file" accept=".jpg, .jpeg, .pdf" placeholder="files" />
            <input class="srcClass" id="srcMarca" type="text" placeholder="marca" />
            <input class="srcClass" id="srcModel" type="text" placeholder="model" />
            <div id="insertB" class="btnStyle">INSERT</div>
        </div><div>
            <div>Update inputs for Cars</div>
            <input class="srcClassId" type="text" placeholder="id" />
            <input class="srcClassFkUser"  type="text" placeholder="fk_user" />
            <input class="srcClassFiles"  type="file" placeholder="files" />
            <input class="srcClassMarca"  type="text" placeholder="marca" />
            <input class="srcClassModel"  type="text" placeholder="model" />
            <div id="updateBt"  class="btnStyle">UPDATE</div>
             <div id="deleteBt"  class="btnStyle">DELETE</div>
        </div>
    </div>
</div>
<br /><br /><br />
<div>View Table:</div>
<div id="result" style="max-height: 300px;overflow-y: scroll;"> </div>




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



<script>        

    let srcId = $(".srcClassId");
    let srcIdUpdate;

    let srcFkUser = $(".srcClassFkUser");
    let srcFkUserUpdate;

    let srcFiles = $(".srcClassFiles");
    let srcFilesUpdate;

    let srcMarca = $(".srcClassMarca");
    let srcMarcaUpdate;

    let srcModel = $(".srcClassModel");
    let srcModelUpdate;

    let result = document.getElementById("result");
    let insertB = $("#insertB");
    let searchB = $("#searchB");  //search
    let orderDirection = "ASC";
    let orderElement = "id";
    let updateB = $("#updateBt");
    let deleteB = $("#deleteBt");


    


    //   srcNumeFiltru.addEventListener("keyup", () => {
    //     result.textContent = "";
    //      result.textContent = srcNumeFiltru.value;
    //   });


    //jquery selection by id

    

    $(".orderBy").click(function () {
        
       orderElement = $(this).attr("order");
       orderDirection = $(this).attr("direction") == "ASC" ? "DESC" : "ASC";
        
        $(".orderBy").html("ASC");
        $(".orderBy").attr("direction","ASC");
        $(this).html(orderDirection);
        $(this).attr("direction", orderDirection);
        
        functionSelect();
    });
    //functionSelect();
   
    updateB.click(function () {    //update
        update();
    });


    deleteB.click(async function () {    //delete
        delete2();

    });

    insertB.click(function () {  //insert
        
       
        insert();
     
    });

    searchB.click(function () {    //search
        functionSelect();
    });

    $(".srcInput").keyup(function () {
        functionSelect();
    });

    cars -> files

    function smth(files) {

        return toate fisierele cu informatia respectiva
    }


    function insert() {
        $("#result").html("");// curata divul\

        var fileInput = $('#srcFiles')[0].files;

        var form_data = new FormData();
        for (var i = 0; i < fileInput.length; i++) {
            form_data.append("fileFromSmth"+i, fileInput[i]);
        }

        // pregatesti formdata pentru a il trimite in back-end
        form_data.append("fk_user_frontend", $("#srcFkUser").val());
        form_data.append("marca_frontend", $("#srcMarca").val());
        form_data.append("model_frontend", $("#srcModel").val());

        

        $.ajax({
            url: 'brain.asmx/insertCars',// trimiti catre functia select din brain.asmx
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
                functionSelect();
            }
        });

    }

    function functionSelect() {
        $("#result").html("");// curata divul
        var form_data = new FormData(); // pregatesti formdata pentru a il trimite in back-end

        form_data.append("order_front", orderDirection);
        form_data.append("element_front", orderElement);

        form_data.append("id_frontend_filtru", $("#srcIdFiltru").val());
        form_data.append("marca_frontend_filtru", $("#srcMarcaFiltru").val());
        form_data.append("model_frontend_filtru", $("#srcModelFiltru").val());
        // adauga valoarea ta in form
        $.ajax({
            url: 'brain.asmx/selectCars',// trimiti catre functia select din brain.asmx
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
        form_data.append("fk_user_update", $(".srcClassFkUser").val());
        form_data.append("files_update", $(".srcClassFiles").val());
        form_data.append("marca_update", $(".srcClassMarca").val());
        form_data.append("model_update", $(".srcClassModel").val());
        
        

        $.ajax({
            url: 'brain.asmx/updateCars',// trimiti catre functia select din brain.asmx
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
                functionSelect();
            }
        });
    }

  


    function delete2() {
        $("#result").html("");// curata divul
        var form_data = new FormData(); // pregatesti formdata pentru a il trimite in back-end

        form_data.append("id_delete", $(".srcClassId").val());
        
        $.ajax({
            url: 'brain.asmx/deleteCars',// trimiti catre functia select din brain.asmx
            type: 'POST',
            data: form_data,
            dataType: 'json',
            contentType: false,
            cache: false,
            processData: false,
            beforeSend: function () { },
            success: function (data) {
                functionSelect();
            }
        });
    }


    function createRow(e) {
        let info = document.createElement("div");// creezi un element html de tip div
        $(info).click(function () {
            console.log(e.id);
            srcIdUpdate = srcId.val(e.id);
            srcFkUserUpdate = srcFkUser.val(e.fk_user);
            srcFilesUpdate = srcFiles.val(e.files);
            srcMarcaUpdate = srcMarca.val(e.marca);
            srcModelUpdate = srcModel.val(e.model);
        });
        info.style.cssText = `border:1px solid black; border-radius:6px; margin-top:15px; margin-bottom:15px; cursor:pointer;`;
        info.textContent = `${e.id}, ${e.status}, ${e.fk_user}, ${e.files}, ${e.marca}, ${e.model}  `; // populezi continutul divului cu idul si numele randului
        return info;
    }
   


   
   

</script>





</asp:Content>




