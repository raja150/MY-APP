import DataImport from 'domain/Setup/DataImport'
import React from 'react'
const IMPORT_TYPES = [
    { value: 1, text: 'Attendance', postURL: 'Attendance', type: 'Attendance', fileName: '', haveDate: true },
    { value: 2, text: 'Salary', postURL: 'Salaries', type: 'salaries', fileName: '', haveDate: false },
    { value: 3, text: 'EPF ESI', postURL: 'EmpStatutory', type: 'EPFESI', fileName: '', haveDate: false },
    { value: 4, text: 'Paysheet', postURL: 'PaySheet', type: 'PaySheet', fileName: '', haveDate: true },
    { value: 5, text: 'Bonus', postURL: 'Bonus', type: 'Bonus', fileName: '', haveDate: false },
    { value: 6, text: 'Incentives & Pay cut', postURL: 'IncentivesPayCut', type: 'incentives_pay_cut', fileName: '', haveDate: true },
    { value: 7, text: 'Arrear', postURL: 'Arrear', type: 'Arrear', fileName: '', haveDate: true },
    { value: 8, text: 'Payment Info', postURL: 'PaymentInfo', type: 'paymentInfo', fileName: '', haveDate: false },
    {value:9 , text:'Latecomers', postURL: 'Latecomers', type: 'Latecomers', fileName: '', haveDate: true},
    {value:10 , text:'IncomeTax Limit', postURL: 'IncomeTaxLimit', type: 'IncomeTaxLimit', fileName: '', haveDate: true}
    

]
function PayRollDataImport(props) {

    return (
        <DataImport IMPORT_TYPES={IMPORT_TYPES} />
    )
}
export default PayRollDataImport;