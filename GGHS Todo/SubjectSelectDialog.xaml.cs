using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GGHS_Todo
{
    public sealed partial class SubjectSelectDialog : ContentDialog
    {
        public string SelectedSubject { get; set; }
        public List<string> Subject { get; } = new()
        {
            "국어",
            "수학",
            "영어",
            "한국사",
            "통합사회",
            "통합과학",
            "과학탐구실험",
            "스포츠 생활",
            "미술창작",
            "음악연주",
            "전공기초 스페인어",
            "전공기초 일본어",
            "전공기초 중국어",
            "국제관계의 이해",

            "독서",
            "수학Ⅱ",
            "수학과제탐구",
            "과학사",
            "생활과 과학",
            "운동과 건강",
            "창의적문제해결기법",
            "스페인어Ⅰ",
            "중국어Ⅰ",
            "일본어Ⅰ",
            "심화영어Ⅰ",
            "국제경제",
            "국제정치",
            "비교문화",
            "동양근대사",
            "세계 역사와 문화",
            "현대정치철학의 이해",
            "세계 지역 연구",
            "공간 정보와 공간 분석",

            "체육",
            "논리적 글쓰기",
            "스페인어권 문화",
            "일본문화",
            "중국문화",
            "심화영어독해Ⅱ",
            "독서와 의사소통",
            "국제화시대의 한국어",
            "사회탐구방법",
            "한국사회의 이해",
            "통계로 바라보는 국제문제",
            "국제법",
            "세계 문제와 미래사회",
            "윤리학 연습",
        };
        public SubjectSelectDialog()
        {
            InitializeComponent();
            SubjectComboBox.ItemsSource = Subject;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (SubjectComboBox.SelectedIndex is -1)
            {
                args.Cancel = true;
                TextBlock.Visibility = Visibility.Visible;
            }
            if (SubjectComboBox.SelectedItem is string s)
            {
                SelectedSubject = s;
            }
        }

    }
}
