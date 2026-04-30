import DataImport from 'domain/Setup/DataImport'
import React from 'react'
const IMPORT_TYPES = [
    { value: 1, text: 'Coding Incentives ', postURL: 'CodingIncentives', type: 'Coding_Incentives', fileName: '' },

]

function CodingDataImport(props){
    return(
        <DataImport IMPORT_TYPES={IMPORT_TYPES}/>
    )
}
export default CodingDataImport