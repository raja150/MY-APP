import { Saving } from 'components/Loader';
import { useFormikContext } from 'formik';
import React from 'react';
import { Button } from 'reactstrap';

const FormButton = (props) => {
    const { title, color } = props;
    const { isSubmitting, values, setFieldError, setFieldTouched, } = useFormikContext();

    return (
        <Button className="mb-2 mr-2 btn-icon btn-success1" key='buttonSave'
            color={color ? color : 'success'} type="button" onClick={() => {
                Object.keys(values).forEach((keyName, i) => setFieldTouched(keyName))
                props.handleSubmit(values, setFieldError);
            }} name="submit"
            disabled={isSubmitting} >
            {
                isSubmitting ? <Saving /> : <>
                    <i className="pe-7s-paper-plane btn-icon-wrapper font-weight-bold text-black"> </i>
                    {(title ? title : "Save")}
                </>
            }
        </Button>
    )
};

export default FormButton
