import React, {Fragment} from 'react'

const FieldError = (props) => { 
    return (
        <Fragment>
            {
                props.errors && props.errors[props.name] &&
                props.touched && props.touched[props.name] &&
                (
                    <div className="text-danger">
                        {props.errors[props.name]}
                    </div>
                )
            }
        </Fragment>
    )
}
export default FieldError
