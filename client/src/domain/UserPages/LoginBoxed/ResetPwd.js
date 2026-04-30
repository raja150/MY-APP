import { notifyError, notifySaved } from 'components/alert/Toast'
import { Input } from "components/dynamicform/Controls"
import { Form, Formik } from "formik"
import React, { Fragment, useState } from "react"
import PasswordChecklist from "react-password-checklist"
import { Button, Card, CardBody, CardTitle, Col, Row } from "reactstrap"
import * as Yup from 'yup'
import APIService from '../../../services/apiservice'
import { Base64 } from 'js-base64';
import SessionStorageService from 'services/SessionStorage';
import * as formUtil from 'utils/form'
function ResetPwd(props) {

	const user = props.location.pathname.split('/reset/')
	const token = props.location.search.split('?r=')

	const [loading, setLoading] = useState(false)

	let formValues = {
		newPwd: '',
		confirmPwd: '',
		validPassword: false
	}
	const validateYupSchema = Yup.object().shape({
		newPwd: Yup.string().required("This field is required"),
		confirmPwd: Yup.string().when("newPwd", {
			is: val => (val && val.length > 5 ? true : false),
			then: Yup.string().oneOf(
				[Yup.ref("newPwd")],
				"Both password need to be the same"
			).required("Re-enter password")
		})
	})
	const handleClick = async () => {
		props.history.push('/login');
	};
	const handleSubmit = async (values, actions, errors) => {
		setLoading(true)
		const data = {
			newPassword: Base64.encode(values.newPwd),
			userId: user[1],
			token: token[1]
		}

		await APIService.putAsync('Auth/ResetPwd', data).then((response) => {
			notifySaved("Password Updated Successfully");
			SessionStorageService.removeEmpInfo();
			setTimeout(function () {
				window.close();
			}, 3000);
		}).catch((er) => {
			formUtil.displayAPIError(er)
		})
		setLoading(false)
	}
	return (
		<Fragment>
			<div className='d-flex align-items-center justify-content-center p-4 h-100 bg-dark'>
				<Card className='animate__animated animate__zoomIn'>
					<CardBody>
						<Formik
							initialValues={formValues}
							validateYupSchema={validateYupSchema}
							onSubmit={(values, errors) => handleSubmit(values, errors)}
						>
							{({ values, errors, touched, setFieldValue }) => {
								const handleValueChange = async (name, value) => {
									setFieldValue(name, value)
								}
								return (
									<Form>
										<CardTitle>Reset Password</CardTitle>
										<Row className='mb-4'>
											<Col md='6'>
												<Row>
													<Input {...{
														name: 'newPwd', label: 'New Password', type: "password",
														value: values['newPwd'], error: errors['newPwd'], touched: touched['newPwd']
													}} handlevaluechange={handleValueChange} />
												</Row>
												<Row>
													<Input {...{
														name: 'confirmPwd', label: 'Confirm Password', type: "password",
														value: values['confirmPwd'], error: errors['confirmPwd'], touched: touched['confirmPwd']
													}} handlevaluechange={handleValueChange} />
												</Row>
											</Col>
											<Col md={6}>
												<PasswordChecklist
													rules={["length", "specialChar", "number", "capital", "match"]}
													minLength={5}
													value={values.newPwd}
													valueAgain={values.confirmPwd}
													onChange={(isValid) => {
														handleValueChange('validPassword', isValid);
													}}
												/>
											</Col>
										</Row>

										<Button className='mt-2 btn btn-success font-bold' type='submit' disabled={!values.validPassword || loading} color='success'>Reset {values['validPassword']}</Button>
										<Button className='mt-2 ml-2 btn btn-success font-bold' type='button' color='danger' onClick={handleClick}>Cancel</Button>
									</Form>
								)
							}}
						</Formik>
					</CardBody>
				</Card>
			</div>
		</Fragment>
	)

}
export default ResetPwd;