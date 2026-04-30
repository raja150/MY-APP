import React from 'react'
import { Radio, CheckBox, Input, TextAreaInput } from 'components/dynamicform/Controls'
import { TRUE_FALSE } from 'domain/OnlineTest/Questions/Constant'

function AnswerType(props) {
    const getOptions = (choices) => {
        let arr = []
        choices && choices.forEach((x, i) => {
            arr.push({
                value: x.id,
                text: x.text
            })
        })
        return arr;
    }

    switch (props.type) {
        case 1:
            return <Radio {...{
                name: props.name, label: 'Select a correct answer below',
                value: props.value,
                values: getOptions(props.options),
                disabled: props.disabled
            }} handlevaluechange={(name, value) => props.handleOnChange(name, value)} />
        case 2:
            return <CheckBox {...{
                name: props.name, label: 'Select a correct answer below',
                values: getOptions(props.options),
                value: props.value ? props.value : [],
                disabled: props.disabled
            }} handlevaluechange={(name, value) => props.handleOnChange(name, value)} />
        case 3:
            return <Input {...{
                name: props.name, label: 'Answer',
                value: props.value ? props.value : '',
                disabled: props.disabled
            }} handlevaluechange={(name, value) => props.handleOnChange(name, value)} />
        case 4:
            return <Radio {...{
                name: props.name, label: 'Select a correct answer below',
                values: TRUE_FALSE,
                value: props.value,
                disabled: props.disabled
            }} handlevaluechange={(name, value) => props.handleOnChange(name, value)} />
        case 5:
            return <TextAreaInput  {...{
                name: props.name, label: 'Answer',
                value: props.value,
                disabled: props.disabled
            }} handlevaluechange={(name, value) => props.handleOnChange(name, value)} />
        default:
            return <></>
    }
}
export default AnswerType
