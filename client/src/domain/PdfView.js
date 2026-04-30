import React, { useEffect, Fragment, useState } from 'react'
import { Card, CardBody, Label } from 'reactstrap'
import Loading from 'components/Loader';

export default function PdfReader(props) {
    const [data, setData] = useState({ file: '', isLoading: true });
    const { blobData, label } = props

    useEffect(() => {
        const fetchData = async () => {
            const file = new Blob(
                [blobData],
                { type: 'application/pdf' });
            let fileURL = URL.createObjectURL(file);
            setData({ ...data, file: fileURL, isLoading: false })
        }
        fetchData()
    }, [])

    return <Fragment>
        {data.isLoading ? <Loading /> :
            <Card className='m-2'>
                <CardBody>
                    <Label><h6 className='text-info'>{label}</h6></Label>
                    < div>
                        <object width="100%" height="600"
                            data={data.file} type="application/pdf"
                        />
                    </div>
                </CardBody>
            </Card>
        }
    </Fragment>
}
