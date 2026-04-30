import { Field } from 'formik';
import React from 'react';
import { Label } from 'reactstrap';

function LineItem({ form, remove, push, ...props }) {
    const { values, errors, touched, setFieldValue } = form;
    return (
        <table className="table table-bordered table-hover" id="tab_logic">
            <thead>
                <tr>
                    <th className="text-center">
                        <Label htmlFor="entityName">EntityName</Label>
                    </th>
                    <th className="text-center">
                        <Label htmlFor="attribute">Attribute</Label>
                    </th>
                    <th className="text-center">
                        <Label htmlFor="prefix">Prefix</Label>
                    </th>
                    <th className="text-center">
                        <Label htmlFor="nextNo">NextNo</Label>
                    </th>
                </tr>
            </thead>
            <tbody>
                {
                    values.lineItems.length > 0 &&
                    values.lineItems.map((lineItem, index) => {

                        return (
                            <tr>
                                <td>
                                    {/* <Field type="text" readOnly name={`lineItems.${index}.entityName`} className={`form-control ${touched.entityName && errors.entityName ? "is-invalid" : ""}`}
                                        tabIndex="-1" value={lineItem.entityName} /> */}
                                    <Label name={`lineItems.${index}.entityName`} value={lineItem.entityName} >{lineItem.entityName}</Label>
                                </td>
                                <td>
                                    {/* <Field type="text" readOnly name={`lineItems.${index}.attribute`} className={`form-control ${touched.attribute && errors.attribute ? "is-invalid" : ""}`}
                                        tabIndex="-1" value={lineItem.attribute} /> */}
                                    <Label name={`lineItems.${index}.attribute`} value={lineItem.attribute} >{lineItem.attribute}</Label>
                                </td>
                                <td>
                                    <Field type="text" name={`lineItems.${index}.prefix`} className={`form-control ${touched.prefix && errors.prefix ? "is-invalid" : ""}`}
                                        tabIndex="-1" value={lineItem.prefix} />
                                </td>
                                <td>
                                    <Field type="text" name={`lineItems.${index}.nextNo`} className={`form-control ${touched.nextNo && errors.nextNo ? "is-invalid" : ""}`}
                                        tabIndex="-1" value={lineItem.nextNo} />
                                </td>
                            </tr>
                        )
                    })
                }

                <tr>
                    <td>
                        {/* <Field type="text" readOnly name='entityName' className={`form-control ${touched.entityName && errors.entityName ? "is-invalid" : ""}`}
                            tabIndex="-1" value={values.entityName} /> */}
                    </td>
                    <td>
                        {/* <Field type="text" readOnly name='attribute' className={`form-control ${touched.attribute && errors.attribute ? "is-invalid" : ""}`}
                            tabIndex="-1" value={values.attribute} /> */}
                    </td>
                    <td>
                        {/* <Field type="text" name='prefix' className={`form-control ${touched.prefix && errors.prefix ? "is-invalid" : ""}`}
                            tabIndex="-1" value={values.prefix} /> */}
                    </td>
                    <td>
                        {/* <Field type="text" name='nextNo' className={`form-control ${touched.nextNo && errors.nextNo ? "is-invalid" : ""}`}
                            tabIndex="-1" value={values.nextNo} /> */}
                    </td>
                </tr>
            </tbody>
        </table>
    )
}

export default LineItem
