import { Saving } from 'components/Loader';
import { useFormikContext } from 'formik';
import React, { Fragment, useContext, useEffect, useState } from 'react';
import SweetAlert from 'react-bootstrap-sweetalert';
import { Button } from 'reactstrap';
// import { ControlsContext } from 'providers/ControlsProvider';


const ConfirmAlertCallBack = (props) => {
    const { title, confirmText, color, message, disabled } = props;
    const { isSubmitting, setSubmitting, values, errors, setFieldValue, setFieldTouched, touched } = useFormikContext();

    // const { controlState, dispatch } = useContext(ControlsContext);
    // const { saveState } = controlState;

    const [popup, setPopup] = useState(false);

    useEffect(() => {
        setPopup(false)
    }, [])

    const sweetAlertBox = {
        position: 'absolute',
        width: 'auto',
        minWidth: '250px',
        margin: 0,
        left: "40%",
        top: "45%"
    }

    const onConfirm = () => {
        // if (errors) {
        //     dispatch({ type: 'Save', payload: true })
        // } else {
        //     dispatch({ type: 'Save', payload: false })
        // }
        setPopup(true);
    }

    const onClosePopup = () => {
        // dispatch({ type: 'Save', payload: false });
        setPopup(false);
    }

    return (
        <Fragment>
            <SweetAlert
                show={popup}
                custom
                showCancel
                showCloseButton
                confirmBtnText={"Yes"}
                cancelBtnText={"No"}
                confirmBtnBsStyle="primary"
                cancelBtnBsStyle="light"
                title={title || 'You want to approve?'}
                // customClass="sweetalertBox"
                style={sweetAlertBox}
                onConfirm={() => {
                    if (!isSubmitting) {
                        Object.keys(values).forEach((keyName, i) => setFieldTouched(keyName))
                        setFieldTouched('');
                        //false -> Making previous question false in Test screen
                        //True -> To finish the exam in Test screen
                        props.handleSubmit(values, setFieldValue, false, true);
                    }
                    onClosePopup()
                }}
                onCancel={() => onClosePopup()}
            >
                {message}
            </SweetAlert>

            <Button block className="mb-2 mr-2 btn-icon btn-success1" key='buttonSave'
                color={color ? color : 'success'} type="button" onClick={() => onConfirm()} name="submit"
                disabled={isSubmitting || disabled} >
                {
                    isSubmitting ? <Saving /> : <>
                        <i className="pe-7s-paper-plane btn-icon-wrapper font-weight-bold text-black"> </i>
                        {(confirmText ? confirmText : "Save")}
                    </>
                }
            </Button>
        </Fragment>
    )
};

export default ConfirmAlertCallBack
