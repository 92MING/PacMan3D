<<<<<<< HEAD
import { useRef, useEffect, useState } from 'react';
import { InputTag } from '@arco-design/web-react';
import enUS from '@arco-design/web-react/es/locale/en-US';
import {IconUpload} from '@arco-design/web-react/icon'
import {
  Form,
  AutoComplete,
  Input,
  Select,
  TreeSelect,
  Button,
  Checkbox,
  Switch,
  Radio,
  Cascader,
  Message,
  InputNumber,
  Rate,
  Slider,
  Upload,
  DatePicker,
  Modal,
} from '@arco-design/web-react';
import './own_blog.css'


const FormItem = Form.Item;
const cascaderOptions = [
  {
    value: 'beijing',
    label: 'Beijing',
    children: [
      {
        value: 'beijingshi',
        label: 'Beijing',
        children: [
          {
            value: 'chaoyang',
            label: 'Chaoyang',
            children: [
              {
                value: 'datunli',
                label: 'Datunli',
              },
            ],
          },
        ],
      },
    ],
  },
  {
    value: 'shanghai',
    label: 'Shanghai',
    children: [
      {
        value: 'shanghaishi',
        label: 'Shanghai',
        children: [
          {
            value: 'huangpu',
            label: 'Huangpu',
          },
        ],
      },
    ],
  },
];
=======
import { useRef, useEffect, useState,useContext } from 'react';
import { InputTag } from '@arco-design/web-react';
import enUS from '@arco-design/web-react/es/locale/en-US';
import {IconUpload} from '@arco-design/web-react/icon'
import { AuthContext } from "../component/AuthContext";
import axios from 'axios';


import {
  Form,
  Input,
  Button,
  Message,
} from '@arco-design/web-react';
import './own_blog.css'

const FormItem = Form.Item;

>>>>>>> master
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
<<<<<<< HEAD
  const [locale, setLocale] = useState('en-US');
  const formRef = useRef();
  const [size, setSize] = useState('default');
=======
  const [locale, setLocale] = useState('enUS');
  const formRef = useRef();
  const [size, setSize] = useState('default');
  const { user } = useContext(AuthContext);
  const [blog_title, setTitle] = useState('');
  const [blog_content, setContent] = useState('');

>>>>>>> master
  useEffect(() => {
    formRef.current.setFieldsValue({
      rate: 5,
    });
  }, []);

  const onValuesChange = (changeValue, values) => {
    console.log('onValuesChange: ', changeValue, values);
  };

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
<<<<<<< HEAD
        <FormItem label='Title' field='title' rules={[{required:true}]}>
=======
        <FormItem label='Title' field='title' rules={[{
            validator(value, cb) {
              if (value == null) {
                return cb('Title cannot be empty');
              }

              return cb();
            },
          },]}>
>>>>>>> master
        <Input
          maxLength={{ length: 30, errorOnly: true }}
          showWordLimit
          defaultValue='More than 30 letters will be error'
<<<<<<< HEAD
        />
        </FormItem>
        <FormItem label='Blog' field='blog' rules={[{ required: true }]}>
=======
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
>>>>>>> master
        <Input.TextArea
          autoSize
          maxLength={{ length: 9999, errorOnly: true }}
          showWordLimit
          placeholder='Put your idea here. No MORE THAN 9999 LETTERS.'
          style={{ minHeight:280 }}
<<<<<<< HEAD
        />
        </FormItem>
        <FormItem
          label='Add Your Tag'
          field='tag'
          rules={[{ type: 'array', minLength: 1 }]}
        >
         <InputTag
      allowClear
      labelInValue
      defaultValue={[
        {
          label: 'default_test_tag',
          value: '1',
        },
      ]}
      dragToSort
      placeholder='Please input'
      onChange={(v) => {
        console.log(v);
      }}
    />
        </FormItem>
        <Form.Item
          label='Upload Images'
          field='upload'
          triggerPropName='fileList'
          initialValue={[
            {
              uid: '-1',
              url: '//p1-arco.byteimg.com/tos-cn-i-uwbnlip3yd/e278888093bef8910e829486fb45dd69.png~tplv-uwbnlip3yd-webp.webp',
              name: '20200717',
            },
          ]}
        >
          <Upload
            listType='picture-card'
            multiple
            name='files'
            action='/'
            onPreview={(file) => {
              Modal.info({
                title: 'Preview',
                content: (
                  <img
                    src={file.url || URL.createObjectURL(file.originFile)}
                    style={{
                      maxWidth: '100%',
                    }}
                  ></img>
                ),
              });
            }}
          />
        </Form.Item>
        <FormItem {...noLabelLayout}>
          <Button
            onClick={async () => {
              if (formRef.current) {
                try {
                  await formRef.current.validate();
                  Message.info('Success！');
                } catch (_) {
                  console.log(formRef.current.getFieldsError());
                  Message.error('Failed！');
                }
              }
=======
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
                if (res.data.isAdded ) {
                  Message.success('Success！');
                } else{
                  Message.error('Failed！');
                }
              } catch (e) {
                Message.error('Server is down！');
                console.error(e);
              }
>>>>>>> master
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