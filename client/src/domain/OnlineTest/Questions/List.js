import { RWDropdownList } from 'components/dynamicform/Controls';
import DataTable from 'components/table/DataTable';
import UpdateTable from 'domain/HOC/withDataTable';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, Card, CardBody, Col, Row } from 'reactstrap';
import QuestionService from 'services/OnlineTest/Question';
import PaperService from 'services/OnlineTest/Paper/index'
import { QUES_TYPES } from './Constant';

const title = 'Questions  '

const fields = [
    { name: 'paper', label: 'Paper', type: 'input', },
    { name: 'type', label: 'Question Type', type: 'input' },

    {
        name: 'text', label: 'Question', type: 'custom',
        Cell: ({ value }) => {
            return <div dangerouslySetInnerHTML={{ __html: value }}></div>
        }
    },
]

const ddData = ''

function QuestionsList(props) {

    const [state, setState] = useState({ papers: [], isLoading: true })

    useEffect(() => {
        const fetch = async () => {
            let papers = [];
            await PaperService.GetPapers().then((res) => {
                papers = res.data;
            })
            setState({ ...state, papers: papers, isLoading: false })
        }
        fetch()
    }, [])

    const searchCard = () => {
        return <Fragment>
            <Card className="main-card mb-2">
                <CardBody>
                    <Row form>
                        <Col md="3">
                            <RWDropdownList {...{
                                name: 'paperId', label: 'Paper',
                                valueField: 'id', textField: 'name',
                                values: state.papers,
                                value: props.searchData && props.searchData.paperId,
                            }} handlevaluechange={props.handleOnChange} />
                        </Col>
                        <Col md="3">
                            <RWDropdownList {...{
                                name: 'type', label: 'Type',
                                valueField: 'id', textField: 'text',
                                values: QUES_TYPES,
                                value: props.searchData && props.searchData.type,
                            }} handlevaluechange={props.handleOnChange} />
                        </Col>

                        <Col md="1" style={{ marginTop: '20px' }}>
                            <Button className="mr-2  btn-icon btn-icon-only btn-secondary btn-sm"
                                onClick={() => props.handleSearch()}>
                                <i className="pe-7s-search btn-icon-wrapper" />
                            </Button>
                        </Col>
                    </Row>
                </CardBody>
            </Card>
        </Fragment>
    }
    return (
        <DataTable {...props} searchCard={searchCard} hideAdd={true} />
    )
}
export default UpdateTable(QuestionsList, { ...QuestionService.paginate() }, fields, ddData, title)