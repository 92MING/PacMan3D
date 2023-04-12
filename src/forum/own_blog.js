import { useRef, useEffect, useState,useContext } from 'react';
import { InputTag } from '@arco-design/web-react';
import enUS from '@arco-design/web-react/es/locale/en-US';
import {IconUpload} from '@arco-design/web-react/icon'
import { AuthContext } from "../component/AuthContext";
import axios from 'axios';
import { useNavigate } from "react-router-dom";


import {
  Result,
  Form,
  Input,
  Button,
  Message,
} from '@arco-design/web-react';
import './own_blog.css'

const FormItem = Form.Item;

const formItemLayout = {
  labelCol: {
    span: 7,
  },
  wrapperCol: {
    span: 17,
  },
};
const noLabelLayout = {
  wrapperCol: {
    span: 17,
    offset: 7,
  },
};

function App() {
  const [locale, setLocale] = useState('enUS');
  const formRef = useRef();
  const [size, setSize] = useState('default');
  const { user } = useContext(AuthContext);
  const [blog_title, setTitle] = useState('');
  const [blog_content, setContent] = useState('');

  useEffect(() => {
    formRef.current.setFieldsValue({
      rate: 5,
    });
  }, []);

  const onValuesChange = (changeValue, values) => {
    console.log('onValuesChange: ', changeValue, values);
  };


  const navigate = useNavigate();

  const handleBackClick = () => {
    navigate(0);
  }

  return (
    <div style={{ maxWidth: 650 }}>
      <Form
        ref={formRef}
        autoComplete='off'
        {...formItemLayout}
        size={size}
        initialValues={{
          slider: 20,
          'a.b[0].c': ['b'],
        }}
        onValuesChange={onValuesChange}
        scrollToFirstError
      >
        <FormItem label='Title' field='title' rules={[{
            validator(value, cb) {
              if (value == null) {
                return cb('Title cannot be empty');
              }

              return cb();
            },
          },]}>
        <Input
          maxLength={{ length: 30, errorOnly: true }}
          showWordLimit
          defaultValue='More than 30 letters will be error'
          onChange={(blog_title,e)=>setTitle(e.target.value)}
        />
        </FormItem>
        <FormItem label='Blog' field='blog' rules={[{
            validator(value, cb) {
              if (value == null) {
                return cb('Blog content cannot be empty');
              }

              return cb();
            },
          },]} >
        <Input.TextArea
          autoSize
          maxLength={{ length: 9999, errorOnly: true }}
          showWordLimit
          placeholder='Put your idea here. No MORE THAN 9999 LETTERS.'
          style={{ minHeight:280 }}
          onChange={(blog_content,e)=>setContent(e.target.value)}
        />
        </FormItem>
        
        <FormItem {...noLabelLayout}>
          <Button
            onClick={async (e) => {
              e.preventDefault();
              if (formRef.current) {
                try {
                  await formRef.current.validate();
                } catch (_) {
                  console.log(formRef.current.getFieldsError());
                  Message.error('Failed！');
                  return;
                }
              }
              console.log('title: ', blog_title);
              console.log('blog: ', blog_content);
              console.log('user: ', user);
              const url="http://localhost:3000/api/blog/add"
              try{
                const res = await axios.post(url, { blog_title, blog_content, user });
                if (res.data.isCreated ) {
                  <div>
                  <Result
                    status='success'
                    title='Successfully Submitted！'
                    extra={[
                      <Button key='again' type='secondary' style={{ margin: '0 16px' }}>
                        Again
                      </Button>,
                      <Button key='back' type='primary' onClick={handleBackClick}>
                        Back
                      </Button>,
                    ]}
                  ></Result>
                  </div>
                } else{
                  Message.error('Failed！');
                }
              } catch (e) {
                Message.error('Server is down！');
                console.error(e);
              }
            }}
            type='primary'
            style={{ marginRight: 24 }}
            icon={<IconUpload />}
          >
            Submit
          </Button>
          <Button
            onClick={() => {
              formRef.current.resetFields();
            }}
          >
            Clear ALL
          </Button>
        </FormItem>
      </Form>
    </div>
  );
}

export default App;