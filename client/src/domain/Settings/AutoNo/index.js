import React, { useState, useEffect } from 'react'
import { Button, Card, CardBody, Col, Row, Container, FormGroup, Label } from 'reactstrap';
import { FieldArray, Form, Formik, Field } from 'formik';
import { EmptyItem } from './ConstValues'
import LineItem from './LineItem';
import { Slide, toast } from 'react-toastify';
import SettingsService from '../../../services/Settings';
function Index() {
    const [state, setState] = useState({ sequence: [], isLoading: true });
    let formValues = {};

    useEffect(() => {
        const fetchData = async () => {
            let result = await SettingsService.getSequenceNo().catch((error) => {
                notifyError(error.message);
            });
            setState({ ...state, sequence: result.data, isLoading: false })
        }
        fetchData();
    }, []);

    if (state.sequence.length > 0) {
        formValues.lineItems = [];
        state.sequence.map((item) => { 
            formValues.lineItems.push({
                id: item.id,
                entityName: item.entityName,
                attribute: item.attribute,
                prefix: item.prefix,
                nextNo: item.nextNo
            })
        })
    } else {
        formValues = {
            lineItems: [EmptyItem()],
        }
    }

    const handleSave = async (values, actions, errors) => {
        let items = [];

        values.lineItems.forEach(function (item) {
            if (item) {
                items.push({
                    id:item.id,
                    EntityName: item.entityName,
                    Attribute: item.attribute,
                    Prefix: item.prefix,
                    NextNo: item.nextNo
                });
            }
        })
        try {
            await SettingsService.putSequenceNo(items).catch((error) => { throw new Error(error.message) });
            notifySaved();
        }
        catch (error) {
            notifyError(error.message);
        }
    }

    const notifySaved = () => toast("Data saved successfully!", {
        transition: Slide,
        closeButton: true,
        autoClose: 2500,
        position: 'bottom-center',
        type: 'success'
    });

    const notifyError = (msg) => toast(msg, {
        transition: Slide,
        closeButton: true,
        autoClose: 2500,
        position: 'bottom-center',
        type: 'error'
    });


    return (
        state.isLoading ? '' :
            <Row>
                <Col md="12">
                    <Card className="mb-3">
                        <CardBody>
                            <Formik
                                initialValues={formValues}
                                onSubmit={(values, actions, errors) => handleSave(values, actions, errors)}
                            >
                                {({ formik, touched, errors, isSubmitting, values, setFieldValue }) => {
                                    return (
                                        <Form>
                                            <FieldArray
                                                name="lineItems"
                                                component={LineItem}
                                            />
                                            <Button>Submit</Button>
                                        </Form>
                                    )
                                }}

                            </Formik>

                        </CardBody>
                    </Card>
                </Col>

            </Row>
    )
}

export default Index
