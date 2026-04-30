import DataImport from 'domain/Setup/DataImport'
import React from 'react'
const IMPORT_TYPES = [
    { value: 1, text: 'MT Incentives', postURL: 'MTIncentives', type: 'MT_Incentives', fileName: '' }
]

function TranscriptionDataImport(props){
    return(
        <DataImport IMPORT_TYPES={IMPORT_TYPES}/>
    )
}
export default TranscriptionDataImport