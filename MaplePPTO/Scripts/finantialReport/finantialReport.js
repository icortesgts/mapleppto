let anio = 1;
let count = 0;

const columnHeaderDetail = listHeaderDetail.map((x, index) => {
    let field = "";

    if (x.includes("Total")) {
        field = "anio" + anio;
        anio++;
        count--;
    }
    else {
        field = "mes" + ((index+1) + count);
    }
        

    return { title: x, field: field, hozAlign: "right" }
});

let table;

window.addEventListener('load', function (event) {
    const tipoReporte = document.getElementById("ddlTiporeporte");
    if (tipoReporte.value > 18) {
        genTableDetailHeaderOther();
        
    } else {
        genTableDetailHeader();
    }

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
    const presupuesto = document.getElementById("ddlBudgets");
    const escenario = document.getElementById("ddlEscenaries");
    const model = {
        PresupuestoId: presupuesto.value,
        TipoReporteId: tipoReporte.value,
        EscenarioId: escenario.value
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

            //if (tipoReporte.value > 18) {
            //    table.showColumn("Cliente");
            //    table.showColumn("ciudad");
            //}


            table.setData(data.data.map(x => genDataReportDetail(x)));
        })
        .catch( error => alert(error));
}


const genDataReportDetail = (item) => {

    return {

        "Ppto": item.Ppto,
        "Escenario": item.Escenario,
        "Cliente": item.Cliente,
        "ciudad": item.ciudad,

        "concepto": item.concepto,



        "anio1": parseFloat(item.anio1).toFixed(2),
        "anio2": parseFloat(item.anio2).toFixed(2),
        "anio3": parseFloat(item.anio3).toFixed(2),
        "anio4": parseFloat(item.anio4).toFixed(2),
        "anio5": parseFloat(item.anio5).toFixed(2),

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
        "grupo": item.grupo,
        "subGrupo": item.subGrupo,
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

function genTableDetailHeader() {
     table = new Tabulator("#example2", {
        height: "550px",
        //reactiveData: true,
        data: [], //load data into table
        //renderHorizontal: "virtual",
        pagination: "local",
        paginationSize: 1000,
        paginationCounter: "rows",
        columns: [
            //{ title: "PRESUPUESTO", field: "Ppto" },, frozen: true
            //{ title: "ESCENARIO", field: "Escenario" },
            { title: "CLIENTE", field: "Cliente", headerFilter: "input", frozen: true, visible: false },
            { title: "CIUDAD", field: "ciudad", headerFilter: "input", frozen: true, visible: false },
            { title: "CONCEPTO", field: "concepto", headerFilter: "input", frozen: true },
            ...columnHeaderDetail
        ],
        groupBy: function (data) {
            //data - the data object for the row being grouped
            let text = "";
            switch (data.grupo) {
                case 1: text = "REVENUE"; break;
                case 2: text = "DIRECT COST"; break;
                case 3: text = "GENERAL EXPENSES"; break;
            }

            return text; //groups by data and age
        }
    });

}

function genTableDetailHeaderOther() {
    table = new Tabulator("#example2", {
        height: "550px",
        //reactiveData: true,
        data: [], //load data into table
        //renderHorizontal: "virtual",
        pagination: "local",
        paginationSize: 1000,
        paginationCounter: "rows",
        columns: [
            //{ title: "PRESUPUESTO", field: "Ppto" },, frozen: true
            //{ title: "ESCENARIO", field: "Escenario" },
            { title: "CLIENTE", field: "Cliente", headerFilter: "input", frozen: true },
            { title: "CIUDAD", field: "ciudad", headerFilter: "input", frozen: true },
            { title: "CONCEPTO", field: "concepto", headerFilter: "input", frozen: true },
            ...columnHeaderDetail
        ],
    });

}

