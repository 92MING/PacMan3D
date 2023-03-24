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
  const [locale, setLocale] = useState('en-US');
  const formRef = useRef();
  const [size, setSize] = useState('default');
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
        <FormItem label='Blog' field='blog' rules={[{ required: true }]}>
        <Input.TextArea
          autoSize
          maxLength={{ length: 9999, errorOnly: true }}
          showWordLimit
          placeholder='Put your idea here. No MORE THAN 9999 LETTERS.'
          style={{ minHeight:300 }}
        />
        </FormItem>
        <FormItem
          label='Add Your Tag'
          field='a.b[0].c'
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