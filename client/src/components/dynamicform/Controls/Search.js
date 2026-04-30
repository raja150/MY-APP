import { notifyError } from 'components/alert/Toast';
import React, { useRef, useState } from 'react';
import { AsyncTypeahead as AsyncTypAHead } from 'react-bootstrap-typeahead';
import { FormGroup } from 'reactstrap';
import APIService from 'services/apiservice';
import * as camelCase from 'camelcase';

function AsyncSearch(props) {
    const [isLoading, setIsLoading] = useState(false);
    const [searchData, setSearchData] = useState([]);
    const asyncRef = useRef('');
    const { label, name, url, selData,asyncDwName, textField, touched, error, } = props

    return (
        <FormGroup>
            <label className="text-capitalize" htmlFor={name}>{label}</label>
            <AsyncTypAHead
                name={name}
                selectHintOnEnter
                onSearch={async (query) => {
                    setIsLoading(true);
                    await APIService.getAsync(`${url}/${query}`).then(res => {
                        setSearchData(res.data)
                    }).catch(err => {
                        notifyError(err.message)
                    })
                    setIsLoading(false);
                }}
                onChange={async (e) => {
                    if (e.length > 0) {
                        const selData = e[0];
                        await props.handlevaluechange(name, selData.id, {})
                        await props.handlevaluechange(asyncDwName, selData, {})
                    } else {
                        await props.handlevaluechange(name, '', {})
                        await props.handlevaluechange(asyncDwName, {}, {})
                    }
                }}
                selected={selData && selData.id ? [selData] : []}
                isLoading={isLoading}
                labelKey={option => `${option[camelCase(textField)]}`}
                options={searchData}
                ref={asyncRef}
                clearButton
                id='id'
            />
            {touched && error ?
                <div className="invalid-feedback">
                    {error}
                </div>
                : ''}
        </FormGroup>
    )
}

export default AsyncSearch
