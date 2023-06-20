const columnHeaderDetail = listHeaderDetail.map((x, index) => {
    let field = "";

    if (index < 6) field = "mesan" + (6 - index);
    else field = "mes" + (index - 5);

    return { title: x, field: field, hozAlign: "right" }
});

const table = new Tabulator("#example2", {
    height: "400px",
    //reactiveData: true,
    data: [], //load data into table
    //renderHorizontal: "virtual",
    pagination: "local",
    paginationSize: 1000,
    paginationCounter: "rows",
    columns: [
        //{ title: "PRESUPUESTO", field: "Ppto" },, frozen: true
        //{ title: "ESCENARIO", field: "Escenario" },
        { title: "CLIENTE", field: "Cliente", headerFilter: "input", frozen: true},
        { title: "CIUDAD", field: "ciudad", headerFilter: "input", frozen: true},
        //{ title: "CONTRATO", field: "Contrato" },
        { title: "CONCEPTO", field: "concepto", headerFilter: "input", frozen: true},
        { title: "FASE", field: "Fase", headerFilter: "input", frozen: true},
        ...columnHeaderDetail
    ],
});

window.addEventListener('load', function (event) {

    GetDataToReportDetail();

    //table.setData("/exampledata/ajax");
});
//trigger download of data.csv file
document.getElementById("download-csv").addEventListener("click", function () {
    table.download("csv", "data.csv");
});

//trigger download of data.json file
document.getElementById("download-json").addEventListener("click", function () {
    table.download("json", "data.json");
});

//trigger download of data.xlsx file
document.getElementById("download-xlsx").addEventListener("click", function () {
    table.download("xlsx", "data.xlsx", { sheetName: "My Data" });
});
const GetDataToReportDetail = () => {
    const tipoReporte = document.getElementById("ddlTiporeporte");
    const presupuesto = document.getElementById("ddlPresupuesto");
    const escenario = document.getElementById("ddlEscenaries");
    const showMonths = document.getElementById("hdnShowMonth");
    const model = {
        PresupuestoId: presupuesto.value,
        TipoReporteId: tipoReporte.value,
        EscenarioId: escenario.value,
        mesesFaltantesAnioBase: mesesFaltantes,
        showMonths: showMonths.value,
    };

    fetch(urlDetailsOperationReport, {
        method: 'POST', // or 'PUT'
        body: JSON.stringify({model}),
        headers: {
            'Content-Type': 'application/json',
        },
        
    })
        .then((response) => response.json())
        .then(data => {
            /*tabledata.push(...data.data);*/
            table.setData(data.data.map(x => genDataReportDetail(x)));
        })
        .catch( error => alert(error));
}

//=============================>
//-codigo usando librerias Jquery

$(function () {

    $('.rdnShowMonth').on('click', function (event) {
        console.log("", event.target);
        $('#hdnShowMonth').val(event.target.value);
        GenColumsShow(table, event.target.value);
    });
    
    var gridSummary = $("#gridSummary").DataTable({

        "responsive": false,
        "lengthChange": false,
        "autoWidth": false,
        "scrollX": true,
        "scrollY": 500,
        "paging": false,
        "buttons": ["csv", "excel"],
        processing: true,
        serverSide: false,
        "ajax": {
            "url": urlGetSummaryOperationReport,
            "type": 'POST',
            "data": function () {
                const cboOptionfilter = document.getElementById("cboOptionFilter");
                const tipoReporte = document.getElementById("ddlTiporeporte");
                const presupuesto = document.getElementById("ddlPresupuesto");
                const escenario = document.getElementById("ddlEscenaries");


                return {
                    PresupuestoId: presupuesto.value,
                    TipoReporteId: tipoReporte.value,
                    EscenarioId: escenario.value,
                    summaryReporType: cboOptionFilter.value,
                    mesesFaltantesAnioBase: mesesFaltantes,

                }
            },
        }


    });

    gridSummary.buttons().container().appendTo('#gridSummary_wrapper .col-md-6:eq(0)');
    const colums = [2, 3, 4];
    gridSummary.columns(colums).visible(false);

    $("#btnSummaryReport").on('click', function (event) {
        const element = event.currentTarget;
        element.children[0].style.visibility = 'visible';
        element.disabled = true;
        const cboOptionfilter = document.getElementById("cboOptionFilter");

       
        if (cboOptionFilter.value == '') {
            alert("You must select an option.");
            element.children[0].style.visibility = 'hidden';
            element.disabled = false;
            return false;
        }

        $('#gridSummary').DataTable().ajax.reload(function () {
            if (cboOptionFilter.value == 1) {
                const colHidden = [2, 3];
                gridSummary.columns(colHidden).visible(false);
            }
            else if (cboOptionFilter.value == 2) {
                const colHidden = [2];
                const colShow = [3];
                gridSummary.columns(colHidden).visible(false);
                gridSummary.columns(colShow).visible(true);
            } else if (cboOptionFilter.value == 3) {
                const colHidden = [3];
                const colShow = [2];
                gridSummary.columns(colHidden).visible(false);
                gridSummary.columns(colShow).visible(true);
            }
            element.children[0].style.visibility = 'hidden';
            element.disabled = false;
        }, true);
        gridSummary.buttons().container().appendTo('#gridSummary_wrapper .col-md-6:eq(0)');

        




   

    });


});


//-fin codigo usando librerias Jquery
//<=============================



const genParams = () => {
    const cboOptionfilter = document.getElementById("cboOptionFilter");
    const tipoReporte = document.getElementById("ddlTiporeporte");
    const presupuesto = document.getElementById("ddlPresupuesto");
    const escenario = document.getElementById("ddlEscenaries");

    return {
        PresupuestoId: presupuesto.value,
        TipoReporteId: tipoReporte.value,
        EscenarioId: escenario.value,
        summaryReporType: cboOptionFilter.value,
        mesesFaltantesAnioBase: mesesFaltantes

    }
};
//funciones globales
const GenColumsShow = (table, totalColumShow) => {
    let colNumber = (parseInt(totalColumShow) || 12) + 7 + (parseInt(mesesFaltantes) || 0);

    let total = (parseInt(totalColumns) || 0) + 7;
    let colums = [...Array(total).keys()].filter(x => (x > (colNumber - 1)));

    table.columns().visible(true);
    table.columns(colums).visible(false);
}

const genData = (arr) => {

    return {

        "Ppto": arr[0].Ppto,
        "Escenario": arr[0].Escenario,
        "Cliente": arr[0].Cliente,
        "ciudad": arr[0].ciudad,
        "Contrato": "",
        "concepto": arr[0].concepto,
        "Fase": arr[0].Fase,

        ...(mesesFaltantes == 6 && { "mesant6": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mesant6); }, 0) }),
        ...(mesesFaltantes >= 5 && { "mesant5": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mesant5); }, 0) }),
        ...(mesesFaltantes >= 4 && { "mesant4": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mesant4); }, 0) }),
        ...(mesesFaltantes >= 3 && { "mesant3": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mesant3); }, 0) }),
        ...(mesesFaltantes >= 2 && { "mesant2": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mesant2); }, 0) }),
        ...(mesesFaltantes >= 1 && { "mesant1": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mesant1); }, 0) }),



        //"mesant6": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mesant6); }, 0),
        //"mesant5": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mesant5); }, 0),
        //"mesant4": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mesant4); }, 0),
        //"mesant3": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mesant3); }, 0),
        //"mesant2": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mesant2); }, 0),
        //"mesant1": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mesant1); }, 0),

        "mes1": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes1); }, 0),
        "mes2": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes2); }, 0),
        "mes3": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes3); }, 0),
        "mes4": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes4); }, 0),
        "mes5": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes5); }, 0),
        "mes6": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes6); }, 0),
        "mes7": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes7); }, 0),
        "mes8": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes8); }, 0),
        "mes9": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes9); }, 0),
        "mes10": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes10); }, 0),
        "mes11": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes11); }, 0),
        "mes12": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes12); }, 0),
        "mes13": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes13); }, 0),
        "mes14": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes14); }, 0),
        "mes15": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes15); }, 0),
        "mes16": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes16); }, 0),
        "mes17": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes17); }, 0),
        "mes18": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes18); }, 0),
        "mes19": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes19); }, 0),
        "mes20": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes20); }, 0),
        "mes21": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes21); }, 0),
        "mes22": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes22); }, 0),
        "mes23": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes23); }, 0),
        "mes24": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes24); }, 0),
        "mes25": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes25); }, 0),
        "mes26": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes26); }, 0),
        "mes27": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes27); }, 0),
        "mes28": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes28); }, 0),
        "mes29": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes29); }, 0),
        "mes30": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes30); }, 0),
        "mes31": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes31); }, 0),
        "mes32": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes32); }, 0),
        "mes33": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes33); }, 0),
        "mes34": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes34); }, 0),
        "mes35": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes35); }, 0),
        "mes36": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes36); }, 0),
        "mes37": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes37); }, 0),
        "mes38": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes38); }, 0),
        "mes39": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes39); }, 0),
        "mes40": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes40); }, 0),
        "mes41": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes41); }, 0),
        "mes42": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes42); }, 0),
        "mes43": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes43); }, 0),
        "mes44": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes44); }, 0),
        "mes45": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes45); }, 0),
        "mes46": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes46); }, 0),
        "mes47": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes47); }, 0),
        "mes48": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes48); }, 0),
        "mes49": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes49); }, 0),
        "mes50": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes50); }, 0),
        "mes51": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes51); }, 0),
        "mes52": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes52); }, 0),
        "mes53": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes53); }, 0),
        "mes54": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes54); }, 0),
        "mes55": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes55); }, 0),
        "mes56": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes56); }, 0),
        "mes57": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes57); }, 0),
        "mes58": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes58); }, 0),
        "mes59": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes59); }, 0),
        "mes60": arr.reduce((accumulator, item) => { return accumulator + parseInt(item.mes60); }, 0),
    }
}

        //fetch(urlGetSummaryOperationReport, {
        //    method: 'POST',
        //    body: JSON.stringify({ model }),
        //    headers: { 'Content-Type': 'application/json' }
        //}).then(response => {
        //    if (response.ok) { return response.json() }
        //    throw new Error(response.statusText);  // throw an error if there's something wrong with the response
        //}).then(data => {


        //    //$("#gridSummary").dataTable().fnClearTable();
        //    //addRowTable(data);
        //    //$("#gridSummary").dataTable().fnDraw();
        //   element.children[0].style.visibility = 'hidden';
        //    element.disabled = false;


        //}).catch(error => {
        //    self.lodingSavePalletSuggested = false;
        //    alert(error);
        //});
const addHeaderTable = (listHeader) => {
    let rowHeader = '';
    rowHeader += `
                    <tr>
	                    <th>PRESUPUESTO</th>
	                    <th>ESCENARIO</th>
	                    <th>CLIENTE</th>
	                    <th>CIUDAD</th>
	                    <th>CONTRATO</th>
	                    <th>CONCEPTO</th>
	                    <th>FASE</th>`;
	for(var item in listHeader){
        rowHeader += `<th class="text-uppercase text-nowrap">${item}</th>`;
	}
    rowHeader = `</tr>`;

    return rowHeader;
}

const addRowTable = (list) => {
    let str = '';
    const total = list.length;
    let i = 0;
    while (i < total) {
        let item = list[i];

        str = `<tr>
		<td>${item.Ppto}</td>
		<td>${item.Escenario}</td>
		<td>${item.Cliente}</td>
		<td>${item.ciudad}</td>
		<td>${item.Contrato}</td>
		<td>${item.concepto}</td>
		<td>${item.Fase}</td>
		${(mesesFaltantes == 6 ? '<td class="text-right">' + Math.round(parseFloat(item.mesant6), 0) + '</td>' : '')}
		${(mesesFaltantes >= 5 ? '<td class="text-right">' + Math.round(parseFloat(item.mesant5), 0) + '</td>' : '')}
		${(mesesFaltantes >= 4 ? '<td class="text-right">' + Math.round(parseFloat(item.mesant4), 0) + '</td>' : '')}
		${(mesesFaltantes >= 3 ? '<td class="text-right">' + Math.round(parseFloat(item.mesant3), 0) + '</td>' : '')}
		${(mesesFaltantes >= 2 ? '<td class="text-right">' + Math.round(parseFloat(item.mesant2), 0) + '</td>' : '')}
		${(mesesFaltantes >= 1 ? '<td class="text-right">' + Math.round(parseFloat(item.mesant1), 0) + '</td>' : '')}
		<td class="text-right">${Math.round(parseFloat(item.mes1), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes2), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes3), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes4), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes5), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes6), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes7), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes8), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes9), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes10), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes11), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes12), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes13), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes14), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes15), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes16), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes17), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes18), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes19), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes20), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes21), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes22), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes23), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes24), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes25), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes26), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes27), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes28), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes29), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes30), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes31), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes32), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes33), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes34), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes35), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes36), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes37), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes38), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes39), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes40), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes41), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes42), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes43), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes44), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes45), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes46), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes47), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes48), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes49), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes50), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes51), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes52), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes53), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes54), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes55), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes56), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes57), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes58), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes59), 0)}</td>
		<td class="text-right">${Math.round(parseFloat(item.mes60), 0)}</td>
	</tr>`;
        const rows = htmlToElements(str);
        //console.log(rows[0]);
        $("#gridSummary").dataTable().fnAddData(rows[0]);
        i++;
    }

}

function htmlToElements(html) {
    var template = document.createElement('template');
    template.innerHTML = html;

    return template.content.childNodes;
}
const genDataReportDetail = (item) => {

    return {

        "Ppto": item.Ppto,
        "Escenario": item.Escenario,
        "Cliente": item.Cliente,
        "ciudad": item.ciudad,
        "Contrato": "",
        "concepto": item.concepto,
        "Fase": item.Fase,


        "mesant6": parseFloat(item.mesant6).toFixed(2),
        "mesant5": parseFloat(item.mesant5).toFixed(2),
        "mesant4": parseFloat(item.mesant4).toFixed(2),
        "mesant3": parseFloat(item.mesant3).toFixed(2),
        "mesant2": parseFloat(item.mesant2).toFixed(2),
        "mesant1": parseFloat(item.mesant1).toFixed(2),

        "mes1": parseFloat(item.mes1).toFixed(2),
        "mes2": parseFloat(item.mes2).toFixed(2),
        "mes3": parseFloat(item.mes3).toFixed(2),
        "mes4": parseFloat(item.mes4).toFixed(2),
        "mes5": parseFloat(item.mes5).toFixed(2),
        "mes6": parseFloat(item.mes6).toFixed(2),
        "mes7": parseFloat(item.mes7).toFixed(2),
        "mes8": parseFloat(item.mes8).toFixed(2),
        "mes9": parseFloat(item.mes9).toFixed(2),
        "mes10": parseFloat(item.mes10).toFixed(2),
        "mes11": parseFloat(item.mes11).toFixed(2),
        "mes12": parseFloat(item.mes12).toFixed(2),
        "mes13": parseFloat(item.mes13).toFixed(2),
        "mes14": parseFloat(item.mes14).toFixed(2),
        "mes15": parseFloat(item.mes15).toFixed(2),
        "mes16": parseFloat(item.mes16).toFixed(2),
        "mes17": parseFloat(item.mes17).toFixed(2),
        "mes18": parseFloat(item.mes18).toFixed(2),
        "mes19": parseFloat(item.mes19).toFixed(2),
        "mes20": parseFloat(item.mes20).toFixed(2),
        "mes21": parseFloat(item.mes21).toFixed(2),
        "mes22": parseFloat(item.mes22).toFixed(2),
        "mes23": parseFloat(item.mes23).toFixed(2),
        "mes24": parseFloat(item.mes24).toFixed(2),
        "mes25": parseFloat(item.mes25).toFixed(2),
        "mes26": parseFloat(item.mes26).toFixed(2),
        "mes27": parseFloat(item.mes27).toFixed(2),
        "mes28": parseFloat(item.mes28).toFixed(2),
        "mes29": parseFloat(item.mes29).toFixed(2),
        "mes30": parseFloat(item.mes30).toFixed(2),
        "mes31": parseFloat(item.mes31).toFixed(2),
        "mes32": parseFloat(item.mes32).toFixed(2),
        "mes33": parseFloat(item.mes33).toFixed(2),
        "mes34": parseFloat(item.mes34).toFixed(2),
        "mes35": parseFloat(item.mes35).toFixed(2),
        "mes36": parseFloat(item.mes36).toFixed(2),
        "mes37": parseFloat(item.mes37).toFixed(2),
        "mes38": parseFloat(item.mes38).toFixed(2),
        "mes39": parseFloat(item.mes39).toFixed(2),
        "mes40": parseFloat(item.mes40).toFixed(2),
        "mes41": parseFloat(item.mes41).toFixed(2),
        "mes42": parseFloat(item.mes42).toFixed(2),
        "mes43": parseFloat(item.mes43).toFixed(2),
        "mes44": parseFloat(item.mes44).toFixed(2),
        "mes45": parseFloat(item.mes45).toFixed(2),
        "mes46": parseFloat(item.mes46).toFixed(2),
        "mes47": parseFloat(item.mes47).toFixed(2),
        "mes48": parseFloat(item.mes48).toFixed(2),
        "mes49": parseFloat(item.mes49).toFixed(2),
        "mes50": parseFloat(item.mes50).toFixed(2),
        "mes51": parseFloat(item.mes51).toFixed(2),
        "mes52": parseFloat(item.mes52).toFixed(2),
        "mes53": parseFloat(item.mes53).toFixed(2),
        "mes54": parseFloat(item.mes54).toFixed(2),
        "mes55": parseFloat(item.mes55).toFixed(2),
        "mes56": parseFloat(item.mes56).toFixed(2),
        "mes57": parseFloat(item.mes57).toFixed(2),
        "mes58": parseFloat(item.mes58).toFixed(2),
        "mes59": parseFloat(item.mes59).toFixed(2),
        "mes60": parseFloat(item.mes60).toFixed(2),
    }
}

function gendataFromJSObject(listDetail) {
    try {
        let data = [];
        switch (cboOptionFilter.value) {
            case "1":
                const arrayInput = [...listDetail.groupByToMap(x => x.concepto)].map(x => x[1]);
                data = arrayInput.map(arr => genData(arr));
                break;
            case "2":
                const arrayInput2 = [...listDetail.groupByToMap(x => x.ciudad)].map(x => x[1]);
                data = arrayInput2.reduce((array1, array2, index) => {

                    if (index == 1) {
                        array1 = [...array1.groupByToMap(x => x.concepto)].map(x => x[1]).map(arr => genData(arr));
                    }
                    array2 = [...array2.groupByToMap(x => x.concepto)].map(x => x[1]).map(arr => genData(arr));
                    return array1.concat(array2);
                });
                break;
            case "3":
                const arrayInput3 = [...listDetail.groupByToMap(x => x.Cliente)].map(x => x[1]);
                if (arrayInput3.length > 1) {
                    data = arrayInput3.reduce((array1, array2, index) => {

                        if (index == 1) {
                            array1 = [...array1.groupByToMap(x => x.concepto)].map(x => x[1]).map(arr => genData(arr));
                        }
                        array2 = [...array2.groupByToMap(x => x.concepto)].map(x => x[1]).map(arr => genData(arr));
                        return array1.concat(array2);
                    });
                } else {
                    data = [...listDetail.groupByToMap(x => x.concepto)].map(x => x[1]).map(arr => genData(arr));
                }

                break;
        }
    } catch (e) {
        alert("An error Ocurred!");
        //element.children[0].style.visibility = 'hidden';
        //element.disabled = false;
    }
}
    