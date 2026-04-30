import classNames from 'classnames';
import { notifyError } from 'components/alert/Toast';
import React, { useState, useRef } from 'react'
import { AsyncTypeahead } from 'react-bootstrap-typeahead';
import { FormGroup } from 'reactstrap';
import APIService from 'services/apiservice';

function AsyncTypeHead({ label, name, url, textField, valueField, hide = false, touched, error, extension, ...props }) {

    const [isLoading, setIsLoading] = useState(false);
    const [responseData, setResponseData] = useState({});
    const [details, setDetails] = useState();
    const asyncRef = useRef('');

    const cssClass = classNames(
        "form-control",
        {
            "is-invalid": touched && error
        }
    );

    return (
        <FormGroup>
            <label className="text-capitalize" htmlFor={name}>{label}</label>
            <AsyncTypeahead
                // className={cssClass}
                name={name}
                disabled={hide}
                selectHintOnEnter
                onSearch={async (query) => {
                    setIsLoading(true);
                    if (extension) {
                        await APIService.getAsync(`${url}/${query}/${extension}`).then(res => {
                            setResponseData({});
                            setDetails(res.data)
                        }).catch(err => {
                            notifyError(err.message)
                        })
                    } else {
                        await APIService.getAsync(`${url}/${query}`).then(res => {

                            setResponseData({});
                            setDetails(res.data)
                        }).catch(err => {
                            notifyError(err.message)
                        })
                    }

                    setIsLoading(false);
                }}
                onChange={async (e) => {
                    if (e.length > 0) {
                        const patient = e[0];
                        setResponseData(patient);
                        props.handlevaluechange(name, patient[valueField], patient);
                    } else {
                        setResponseData({});
                        props.handlevaluechange(name, '', '');

                    }
                }}
                selected={responseData && responseData[valueField] ? [responseData] : []}
                isLoading={isLoading}
                labelKey={option => `${option[textField]}`}
                options={details ? details : []}
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

export default AsyncTypeHead
