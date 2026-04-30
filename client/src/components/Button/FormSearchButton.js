import { Saving } from 'components/Loader';
import { useFormikContext } from 'formik';
import React from 'react';
import { Button } from 'reactstrap';

const FormSearchButton = (props) => {
    const { title, color, setModel } = props;
    const { isSubmitting, values, setFieldError, setFieldTouched, validateForm, setFieldValue } = useFormikContext();

    return (
        <Button className="mb-2 mr-2 btn-icon btn-success1" key='buttonSave'
            color={color ? color : 'success'} type="button" onClick={async () => {
                Object.keys(values).forEach((keyName, i) => setFieldTouched(keyName))
                const errors = await validateForm();
                if (Object.keys(errors).length === 0) {
                    await props.handleSubmit(values, setFieldError, setFieldValue)
                } else {
                    if (setModel)
                        setModel(false)
                }
            }} name="submit"
            disabled={isSubmitting} >
            {
                isSubmitting ? <Saving /> : <>
                    {/* <i className="pe-7s-paper-plane btn-icon-wrapper font-weight-bold text-black"> </i> */}
                    {(title ? title : "Search")}
                </>
            }
        </Button>
    )
};

export default FormSearchButton
