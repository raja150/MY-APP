import { RWDropdownList } from "components/dynamicform/Controls";
import DataTable from 'components/table/DataTable';
import UpdateTable from 'domain/HOC/withDataTable';
import queryString from 'query-string';
import React, { useEffect, useState } from 'react';
import { VscOpenPreview } from 'react-icons/vsc';
import { Button, Card, CardBody, Col, Row } from 'reactstrap';
import ResultService from 'services/SelfService/OnlineTest/ResultService';
import * as crypto from 'utils/Crypto';
import { Progress } from 'reactstrap'
import './mouseOver.css'

const title = 'Result '

let fields = []

const ddData = ''

function List(props) {
    const [state, setState] = useState({ papers: [], isLoading: true })

    useEffect(() => {
        if (fields.length === 0) {
            fields.push(
                { name: 'paperName', label: 'Paper', type: 'input', id: 'Paper.Name' },
                { name: 'testDate', label: 'Date', type: 'date', id: 'Paper.AddedAt' },
                {
                    name: 'percentage', label: 'Percentage', type: 'custom',
                    Cell: ({ value }) => {
                        return <div style={{ width: '200px' }}>
                            <Progress className="mt-1" color='success ' value={value} >{value + '%'} </Progress>
                        </div>
                    }
                },
                {
                    name: 'view', label: 'View', type: 'custom', disableSorting: true,
                    Cell: ({ row }) => {
                        return <Button color='info'
                            className='p-1 border-0 mouseover'
                            outline
                            disabled={!row.original.showResult}
                            onClick={() => handleRedirect(row.original.id)}
                        >
                            {!row.original.showResult && <span className='mouseovertext'>Under Evaluation</span>}
                            <VscOpenPreview size='20px' />
                        </Button>
                    }
                }
            )
        }
    }, [])

    useEffect(() => {
        const fetch = async () => {
            let papers = []
            await ResultService.GetPapers().then((res) => {
                papers = res.data;
            })
            setState({ ...state, papers: papers, isLoading: false })
        }
        fetch()
    }, [])

    const handleRedirect = (resultId) => {
        props.history.push(`Summary` + '?' + queryString.stringify(
            {
                r: (crypto.encrypt(resultId)),
                s: (crypto.encrypt('SelfService'))
            }
        ))
    }

    const searchCard = () => {
        return <Card className="main-card mb-2">
            <CardBody>
                <Row form className="w-100">
                    <Col md="3">
                        <RWDropdownList {...{
                            name: 'paperId', label: 'Paper',
                            valueField: 'id', textField: 'paperName',
                            values: state.papers,
                            value: props.searchData && props.searchData.paperId,
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
    }
    return <DataTable {...props} searchCard={searchCard} hideAdd={true} />
}
export default UpdateTable(List, { ...ResultService.paginate() }, fields, ddData, title, true)