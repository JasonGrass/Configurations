using dotnetCampus.Configurations.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;

namespace dotnetCampus.Configurations.Tests
{
    /// <summary>
    /// ר��Ϊ��������ļ����̶�дͬһ�������ļ�����ȷ�Խ��в��ԡ�
    /// </summary>
    [TestClass]
    public class FileConfigurationRepoCriticalDataSafetyTests
    {
        /// <summary>
        /// ��дͬһ�������ļ��Ľ��̱�����ͬ�������ݰ�ȫģʽ��
        /// </summary>
        [ContractTestCase]
        public void MustBeTheSameSefetyMode()
        {
            "�������̶�ʹ�ò���ȫ��ʽ��дͬһ�������ļ�������������⡣".Test(() =>
            {

            });

            "�������̶�ʹ�ð�ȫ��ʽ��дͬһ�������ļ�������������⡣".Test(() =>
            {

            });

            "A ������ʹ�ò���ȫ��ʽ��д���ã�B ����ʹ�ð�ȫ�ķ�ʽ��д���ã�B ���̽��׳��쳣��ʾ����ȫ��".Test(() =>
            {

            });

            "A ������ʹ�ð�ȫ��ʽ��д���ã�B ����ʹ�ò���ȫ�ķ�ʽ��д���ã�B ���̽��׳��쳣��ʾ�ᵼ�� A ����ȫ��".Test(() =>
            {

            });
        }

        /// <summary>
        /// ���̶�д�����ļ��İ�ȫ�Լ�������������߽�����
        /// </summary>
        [ContractTestCase]
        public void CompatibleSefetyMode()
        {
            "A ���������Ȳ���ȫ��ʽ�ȶ�д���ã�B �����԰�ȫ�ķ�ʽ��д���ã�A ���̽��Զ��л�Ϊ��ȫ�Ķ�д��ʽ��".Test(() =>
            {

            });

            "A ���������Ȳ���ȫ��ʽ�ȶ�д���ã�B �����Բ���ȫ�ķ�ʽ��д���ã�A��B ���̶��������Բ���ȫ�ķ�ʽ��д��".Test(() =>
            {

            });

            "A ���������Ȳ���ȫ��ʽ�ȶ�д���ã�B ���������Ȳ���ȫ�ķ�ʽ��д���ã�A��B ���̶��������Բ���ȫ�ķ�ʽ��д��".Test(() =>
            {

            });

            // ��ȫ�����ȼ����ڲ���ȫ�����ȼ�����ֻҪ��һ���������Ȱ�ȫ����ô����ͻ���ð�ȫ�ķ�ʽ��д��
            "A ���������Ȳ���ȫ��ʽ�ȶ�д���ã�B ���������Ȱ�ȫ�ķ�ʽ��д���ã�A��B ���̶����԰�ȫ�ķ�ʽ��д��".Test(() =>
            {

            });

            "A ���������Ȱ�ȫ��ʽ�ȶ�д���ã�B �����Բ���ȫ�ķ�ʽ��д���ã�A ���̽��Զ��л�Ϊ����ȫ�Ķ�д��ʽ��".Test(() =>
            {

            });

            "A ���������Ȱ�ȫ��ʽ�ȶ�д���ã�B �����԰�ȫ�ķ�ʽ��д���ã�A��B ���̶��������԰�ȫ�ķ�ʽ��д��".Test(() =>
            {

            });

            // ��ȫ�����ȼ����ڲ���ȫ�����ȼ�����ֻҪ��һ���������Ȱ�ȫ����ô����ͻ���ð�ȫ�ķ�ʽ��д��
            "A ���������Ȱ�ȫ��ʽ�ȶ�д���ã�B ���������Ȳ���ȫ�ķ�ʽ��д���ã�A��B ���̶����԰�ȫ�ķ�ʽ��д��".Test(() =>
            {

            });

            "A ���������Ȱ�ȫ��ʽ�ȶ�д���ã�B ���������Ȱ�ȫ�ķ�ʽ��д���ã�A��B ���̶��������԰�ȫ�ķ�ʽ��д��".Test(() =>
            {

            });
        }

        /// <summary>
        /// ���ļ�����ʱ��
        /// </summary>
        [ContractTestCase]
        public void FileExists()
        {
            "�������̾�����д��1 ����д��� 2 �������̶������Զ������ݡ�".Test(() =>
            {
                var config1 = new FileConfigurationRepo("exist.dcc").CreateAppConfigurator().Of<FakeConfiguration>();
                var config2 = new FileConfigurationRepo("exist.dcc").CreateAppConfigurator().Of<FakeConfiguration>();

                var value1 = "1";
                config1.Key = value1;
                var value2 = config2.Key;

                Assert.AreEqual(value1, value2);
            });

            "�������̾�����д��1��2 ���̷ֱ�д��� 1��2 �������̶��������Զ������ݡ�".Test(() =>
            {

            });
        }

        /// <summary>
        /// ���ļ��մ���ʱ��
        /// </summary>
        [ContractTestCase]
        public void FileCreating()
        {
            "�������̶�д�����ⲿ���������������Ϣ���ļ��󣬴˽��̿��Զ������ݡ�".Test(() =>
            {

            });

            "�������̾�����д��1 ����д�����ú� 2 �������̶������Զ������ݡ�".Test(() =>
            {

            });
        }

        /// <summary>
        /// ���ļ���ɾ��ʱ��
        /// </summary>
        [ContractTestCase]
        public void FileDeleting()
        {
            "�������̶�д����ɾ�����ļ��󣬴˽��������ѱ�������ݱ���ա�".Test(() =>
            {

            });

            "�������̶�д����ɾ�����ļ��󣬴˽�������δ��������ݻᱻд�뵽�ļ��С�".Test(() =>
            {

            });
        }
    }
}
