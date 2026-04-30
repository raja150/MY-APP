import classNames from 'classnames';
import { notifyError } from 'components/alert/Toast';
import React, { useState, useRef } from 'react'
import { AsyncTypeahead } from 'react-bootstrap-typeahead';
import { FormGroup } from 'reactstrap';
import apiservice from 'services/apiservice';
import * as _ from 'lodash'
function AsyncTypeHead({ label, name, url, textField, valueField, hide = false,
    touched, error, extension, selected, ...props }) {
    const [isLoading, setIsLoading] = useState(false);
    const [selReport, setSelReport] = useState({});
    const [rptData, setReptData] = useState([]);
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
                name={name}
                disabled={hide}
                selectHintOnEnter
                onSearch={async (query) => {
                    setIsLoading(true);
                    if (extension) {
                        await apiservice.getAsync(`${url}/${query}/${extension}`).then(res => {
                            setReptData(res.data)
                        }).catch(err => {
                            notifyError(err.message)
                        })
                    } else {
                        await apiservice.getAsync(`${url}/${query}`).then(res => {
                            setReptData(res.data)
                        }).catch(err => {
                            notifyError(err.message)
                        })
                    }

                    setIsLoading(false);
                }}
                onChange={async (e) => {
                    if (e.length > 0) {
                        const report = e[0];
                        let labelKey = name + 'Key'
                        labelKey = report[textField]
                        props.handleAsyncChange(name, report[valueField], labelKey);
                    } else {
                        props.handleAsyncChange(name, '', '');
                    }
                }}
                selected={!_.isEmpty(selected) ? [selected] :  []}
                isLoading={isLoading}
                labelKey={option => `${option[textField]}`}
                options={(rptData ? rptData : [])}
                ref={asyncRef}
                // clearButton
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
