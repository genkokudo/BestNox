using System;

namespace BestNox.Models
{
    /// <summary>
    /// �G���[��ʂ�ViewModel
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// ���N�G�X�gID
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// ��ʂɃ��N�G�X�gID��\�����邩
        /// ��̏ꍇ�͕\�����Ȃ�
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}