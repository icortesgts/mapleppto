//=============================>
//-codigo usando librerias Jquery


$(function () {


    var table = $("#example2").DataTable({
        "responsive": false,
        "lengthChange": false,
        "autoWidth": false,
        "scrollX": true,
        "scrollY": 500,
        "paging": false,
        fixedColumns: {
            leftColumns: 7
        },
        
        //"buttons": ["csv", "excel", "pdf", "print"],
        processing: true,
        serverSide: false,
        "ajax": {
            "url": urlDetailsOperationReport,
            "type": 'POST',
            "data": function () {

                const tipoReporte = document.getElementById("ddlTiporeporte");
                const presupuesto = document.getElementById("ddlPresupuesto");
                const escenario = document.getElementById("ddlEscenaries");
                const showMonths = document.getElementById("hdnShowMonth");

                return {
                    PresupuestoId: presupuesto.value,
                    TipoReporteId: tipoReporte.value,
                    EscenarioId: escenario.value,
                    mesesFaltantesAnioBase: mesesFaltantes,
                    showMonths: showMonths.value,

                }
            },
        }

    });

    /*table.table().buttons().container().appendTo('#example2_wrapper .col-md-6:eq(0)');*/

    GenColumsShow(table, lastShowMonthValue);

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
        "buttons": ["csv", "excel", "pdf", "print"],
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

$('#exportExcelDetail').on('click', (ev) => {
    ev.preventDefault();
    const tipoReporte = document.getElementById("ddlTiporeporte");
    const presupuesto = document.getElementById("ddlPresupuesto");
    const escenario = document.getElementById("ddlEscenaries");
    const showMonths = document.getElementById("hdnShowMonth");

    const url = ev.currentTarget.href

    const params = `?PresupuestoId=${presupuesto.value}&TipoReporteId=${tipoReporte.value}&EscenarioId=${escenario.value}&mesesFaltantesAnioBase=${mesesFaltantes}&showMonths=${showMonths.value}`;

    location.href = `${url}${params}`;
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