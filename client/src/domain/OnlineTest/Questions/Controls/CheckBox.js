import classNames from 'classnames';
import React from 'react';
import { CustomInput, FormGroup } from 'reactstrap';

const OTCheckBox = (props) => {

    const { dpchange, handlevaluechange, value, label, touched, error, className, name, type, disabled, ...rest } = props;
    const cssName = classNames(
        "",
        {
            "text-danger": touched && error
        },
        className
    );
    return (
        // <FormGroup check>
        //     <label className={classes}>
        //         <input type="checkbox" className="form-check-input" {...field} {...props} />
        //         {label}
        //     </label>
        //     <Error touched={meta.touched} error={meta.error} />
        // </FormGroup>
        //https://codesandbox.io/s/o5pw1vx916
        // defaultChecked having issue, it can assinged value one time in cycle. But field.value is null for intial control render.
        // So, check with field.value and then render control
        value || value == "" ?
            <div>
                <legend className="text-capitalize font-weight-normal">{label}</legend>
                {props.values.map((op, k) => {
                     
                    return <FormGroup key={k} check inline>
                        <CustomInput type="checkbox" invalid={touched && error ? true : false}
                            value={op.value} id={name + k} name={name}
                            //defaultChecked={value ? value == op.value || value.includes(op.value) : false}
                            // checked={value.includes(op.value)}
                            checked={value ? value == op.value || value.includes(op.value) : false}
                            disabled={disabled}
                            label={op.text}
                            onChange={e => {
                                const v = value;
                                let vArr = [];
                                if (v.length > 0) {
                                    vArr = v.split(',');
                                }

                                if (e.target.checked) vArr.push(e.target.value);
                                else {
                                    vArr.splice(vArr.indexOf(e.target.value), 1);
                                }
                                handlevaluechange(name, vArr.join(','), {})
                            }}
                        />
                    </FormGroup>
                }
                )}
                {touched && error ?
                    <div className="invalid-feedback">
                        {error}
                    </div>
                    : ''}
            </div > : ""
    );
};

export default OTCheckBox;
