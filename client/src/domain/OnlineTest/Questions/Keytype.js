import { Radio, TextAreaInput } from 'components/dynamicform/Controls'
import React from 'react'
import { TRUE_FALSE } from './Constant'
import Input from 'components/dynamicform/Controls/Input'
import OTCheckBox from './Controls/CheckBox'


function KeyType(props) {
    const { type, name, value, error, touched, singleCorrect, handleValueChange, multiCorrect } = props
    switch (type) {
        case 1: //single correct
            return <Radio {...{
                name: name, label: 'Key',
                value: value,
                values: singleCorrect,
                touched: touched && touched.key,
                error: error && error.key,
            }} handlevaluechange={handleValueChange} />

        case 2: //multi correct
            return <OTCheckBox {...{
                name: name, label: 'Key',
                values: multiCorrect,
                value: value, disabled: false,
                touched: touched && touched.key,
                error: error && error.key,
            }} handlevaluechange={handleValueChange} />

        case 3: 
            return <Input {...{
                name: name, label: 'Key',
                value: value,
                touched: touched && touched.key,
                error: error && error.key,
            }} handlevaluechange={handleValueChange} />
        case 4:
            return <Radio {...{
                name: name, label: 'Key',
                value: value,
                values: TRUE_FALSE,
                touched: touched && touched.key,
                error: error && error.key,
            }} handlevaluechange={handleValueChange} />
        case 5:
            return <TextAreaInput  {...{
                name: name, label: 'Key',
                value: value, touched: touched && touched.key,
                error: error && error.key,
            }} handlevaluechange={handleValueChange} />
        default:
            return <></>
    }

}

export default KeyType
