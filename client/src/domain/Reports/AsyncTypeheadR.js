import React, { useState } from 'react';
import { AsyncTypeahead } from 'react-bootstrap-typeahead';
import { FormGroup } from 'reactstrap';
import apiservice from 'services/apiservice';

const AsyncTypeaheadR = (props) => {
    const [isLoading, setIsLoading] = useState(false);
    const [details, setDetails] = useState()

    return (
        <FormGroup>
              <label className="text-capitalize" htmlFor={props.name}>{props.label}</label>
            <AsyncTypeahead
                name={props.name}
                selectHintOnEnter
                onSearch={async (query) => {
                    setIsLoading(true);
                    await apiservice.getAsync(props.url).then(response => {
                        setDetails(response.data);
                    });
                    setIsLoading(false);
                }}
                onChange={async (e) => {
                    if (e.length > 0) {
                        props.handlevaluechange(props.name, e[0].name, {});
                    }
                }}
                isLoading={isLoading}
                labelKey={option => `${option.name}`}
                // selected={lineItem && lineItem.product.id ? [lineItem.product] : []}
                options={(details ? details : [])}
                id='id'
                clearButton
            />
        </FormGroup>
    )
}

export default AsyncTypeaheadR