export const MTPerformancePayCut = {
    columns: [
        {
            Header: 'Employee Code', accessor: 'employeeCode'
        },
        {
            Header: 'Year', accessor: 'year'
        },
        {
            Header: 'Month', accessor: 'month'
        },
        {
            Header: 'Late Coming', accessor: 'lateComing'
        },
        {
            Header: 'Internal FeedBack', accessor: 'internalFeedBack'
        },
        {
            Header: 'External FeedBack', accessor: 'externalFeedBack'
        },
        {
            Header: 'Pay cut Total', accessor: 'payCutTotal'
        },
        {
            Header: 'Production', accessor: 'production'
        },
        {
            Header: 'Spot Incentives', accessor: 'spotIncentives'
        },
        {
            Header: 'Punctuality Incentive', accessor: 'punctualityIncentive'
        }
        , {
            Header: 'Star Incentives', accessor: 'starIncentives'
        },
        {
            Header: 'CentumClub', accessor: 'centumClub'
        },
        {
            Header: 'First MinIncentive', accessor: 'firstMinIncentive'
        },
        {
            Header: 'Team Incentives', accessor: 'teamIncentives'
        },
        {
            Header: 'Fax Files And Special Incentives', accessor: 'faxFilesAndSpecialIncentives'
        },
        {
            Header: 'Arrears', accessor: 'arrears'
        },
        {
            Header: 'Sunday', accessor: 'sunday'
        },
        {
            Header: 'Incentives Grand Total', accessor: 'incentivesGrandTotal'
        }
    ],
    searchFields: {
        fields: [
            {
                addValues: false,
                auto: false,
                colSize: 6,
                decimals: false,
                disableDate: false,
                disabled: false,
                displayOrder: 1,
                enableTime: false,
                hidden: false,
                isReference: false,
                jsType: "number",
                label: "Employee Code",
                name: "employeeCode",
                propName: "EmployeeCode",
                required: true,
                showInList: true,
                showInSearch: false,
                type: "Input"
            },
            {
            addValues: false,
            auto: false,
            colSize: 6,
            decimals: false,
            disableDate: false,
            disabled: false,
            displayOrder: 1,
            enableTime: false,
            hidden: false,
            isReference: false,
            jsType: "number",
            label: "Year",
            name: "year",
            propName: "Year",
            required: true,
            showInList: true,
            showInSearch: false,
            type: "Input"
        }]

    }
}

export const MTDayPerformance = {
    columns: [
        {
            Header: 'Employee Code', accessor: 'employeeCode'
        },
        {
            Header: 'Perf Date', accessor: 'perfDate'
        },
        {
            Header: 'Target', accessor: 'target'
        },
        {
            Header: 'MT Actual', accessor: 'mtActual'
        },
        {
            Header: 'MT Medium', accessor: 'mtMedium'
        },
        {
            Header: 'MT Easy', accessor: 'mtEasy'
        },
        {
            Header: 'MT Tough', accessor: 'mtTough'
        },
        {
            Header: 'PR Actual', accessor: 'prActual'
        },
        {
            Header: 'PR Easy', accessor: 'prEasy'
        },
        {
            Header: 'PR Medium', accessor: 'prMedium'
        },
        {
            Header: 'PR Tough', accessor: 'prTough'
        },
        {
            Header: 'AE Actual', accessor: 'aeActual'
        },
        {
            Header: 'AE Easy', accessor: 'aeEasy'
        },
        {
            Header: 'AE Medium', accessor: 'aeMedium'
        },
        {
            Header: 'AE Tough', accessor: 'aeTough'
        },
        {
            Header: 'Performed', accessor: 'performed'
        },
        {
            Header: 'Type', accessor: 'type'
        },
    ], 
    searchFields: {
        fields: [
            {
                addValues: false,
                auto: false,
                colSize: 6,
                decimals: false,
                disableDate: false,
                disabled: false,
                displayOrder: 1,
                enableTime: false,
                hidden: false,
                isReference: false,
                jsType: "number",
                label: "Employee Code",
                name: "employeeCode",
                propName: "EmployeeCode",
                required: true,
                showInList: true,
                showInSearch: false,
                type: "Input"
            },
            {
            addValues: false,
            auto: false,
            colSize: 6,
            decimals: false,
            disableDate: false,
            disabled: false,
            displayOrder: 1,
            enableTime: false,
            hidden: false,
            isReference: false,
            jsType: "number",
            label: "Performance Date",
            name: "perfDate",
            propName: "PerfDate",
            required: true,
            showInList: true,
            showInSearch: false,
            type: "RWDatePicker"
        }]

    }
}
export const MTMonthPerformance ={
    columns: [
    {
        Header: 'Employee Code', accessor: 'employeeCode'
    },
    {
        Header: 'Perf Month', accessor: 'perfMonth'
    },
    {
        Header: 'Perf Year', accessor: 'perfYear'
    },
    {
        Header: 'Target', accessor: 'target'
    },
    {
        Header: 'MT Actual', accessor: 'mtActual'
    },
    {
        Header: 'MT Medium', accessor: 'mtMedium'
    },
    {
        Header: 'MT Easy', accessor: 'mtEasy'
    },
    {
        Header: 'MT Tough', accessor: 'mtTough'
    },
    {
        Header: 'PR Actual', accessor: 'prActual'
    },
    {
        Header: 'PR Easy', accessor: 'prEasy'
    },
    {
        Header: 'PR Medium', accessor: 'prMedium'
    },
    {
        Header: 'PR Tough', accessor: 'prTough'
    },
    {
        Header: 'AE Actual', accessor: 'aeActual'
    },
    {
        Header: 'AE Easy', accessor: 'aeEasy'
    },
    {
        Header: 'AE Medium', accessor: 'aeMedium'
    },
    {
        Header: 'AE Tough', accessor: 'aeTough'
    },
    {
        Header: 'Performed', accessor: 'performed'
    },
],
searchFields: {
    fields: [
        {
            addValues: false,
            auto: false,
            colSize: 6,
            decimals: false,
            disableDate: false,
            disabled: false,
            displayOrder: 1,
            enableTime: false,
            hidden: false,
            isReference: false,
            jsType: "number",
            label: "Employee Code",
            name: "employeeCode",
            propName: "EmployeeCode",
            required: true,
            showInList: true,
            showInSearch: false,
            type: "Input"
        },
        {
        addValues: false,
        auto: false,
        colSize: 6,
        decimals: false,
        disableDate: false,
        disabled: false,
        displayOrder: 1,
        enableTime: false,
        hidden: false,
        isReference: false,
        jsType: "number",
        label: "Year",
        name: "year",
        propName: "Year",
        required: true,
        showInList: true,
        showInSearch: false,
        type: "Input"
    },
    
]
}}