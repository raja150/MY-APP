import DynamicSearch from 'components/dynamicform/DynamicSearch';
import Loading from 'components/Loader';
import { Form, Formik } from 'formik';
import React, { useEffect, useState } from 'react';
import { Button, Card, CardBody, Modal, ModalBody, ModalHeader, Row } from 'reactstrap';
import * as formUtil from 'utils/form';

function SearchPopUp(props) {

    const [initialValues, setIntValues] = useState({});
    const [dpValues, setdpValues] = useState();
    const [state, setState] = useState({ isLoading: true });
    const [isSubmit, setIsSubmit] = useState(false);

    useEffect(() => {
        const fetchData = async () => {
            const iniValues = formUtil.getInitialValues({}, props.fields);
            const ddData = await formUtil.getDropDownData(props.fields);
            const searchParams = Object.keys(props.queryProps);
            //setting searched parameters to initial values
            searchParams.forEach((e) => {
                //checking searched param key is exist
                // or not in query props
                if (e in iniValues) {
                    //updating the searched value to initial value
                    iniValues[e] = props.queryProps[e]
                }
            })
            setIntValues(iniValues);
            setdpValues(ddData);
            setState({ isLoading: false, lookUpData: ddData });
        };

        fetchData();
    }, []);


    const handleCancel = () => {
        props.setModal(!props.modal)
    }

    const saveData = async () => {
        setIsSubmit(true)
        props.handleSearch(0)
        props.setModal(!props.modal)
        setIsSubmit(false)

    };
    return (
        state.isLoading ? <Loading /> :
            <Formik enableReinitialize={true}
                // initialValues={formValues}
                {...{ initialValues }}
                onSubmit={(values, actions, setSubmitting) => {
                    saveData(values, actions);
                }}
            >
                {({ handleSubmit, validateForm, setFieldTouched, isSubmitting, ...formikBag }) => {
                    return (
                        <Form onSubmit={handleSubmit}>
                            <Card className="mb-3">
                                <CardBody>
                                    <Modal isOpen={props.modal} size='200%'>
                                        <ModalHeader><h5 style={{ marginLeft: '160px' }}>Advanced Search</h5></ModalHeader>
                                        <ModalBody>
                                            <DynamicSearch accordion={true} key='form_dynamic'
                                                json={props.fields} lookUpData={dpValues} SearchPlus={true} {...formikBag} handleOnChange={props.handleOnChange} />
                                            <Row className='justify-content-center'>
                                                <Button className="mb-2 mr-2 btn-icon btn-success1" key='buttonSubmit' color="success" type="button" name="submit" onClick={handleSubmit} disabled={isSubmit}>
                                                    <i className="pe-7s-paper-plane btn-icon-wrapper font-weight-bold text-black"> </i>
                                                    {isSubmit ? "Please wait..." : "Submit"}
                                                </Button>
                                                <Button className="mb-2 mr-2 btn-icon btn-primary" key='button' color="success" type="button" onClick={() => handleCancel()} name="Cancel" >
                                                    {"Cancel"}
                                                </Button>
                                            </Row>
                                        </ModalBody>
                                    </Modal>
                                </CardBody>
                            </Card>

                        </Form>
                    )
                }}
            </Formik>

    )
}

export default SearchPopUp
