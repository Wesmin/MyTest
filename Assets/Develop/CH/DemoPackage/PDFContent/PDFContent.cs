using UnityEngine;
using Paroxe.PdfRenderer;
using UnityEngine.UI;
using System.IO;


public class PDFContent : MonoBehaviour
{
    private PDFViewer pdfViewer;
    private void Start()
    {
        pdfViewer = transform.GetComponentInChildren<PDFViewer>(true);
        pdfViewer.FileSource = PDFViewer.FileSourceType.FilePath;
        pdfViewer.LoadOnEnable = false;
        pdfViewer.UnloadOnDisable = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
           
            pdfViewer.LoadDocumentFromFile("Assets/Develop/CH/DemoPackage/PDFContent/PDFRenderer/Documentation.pdf");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            pdfViewer.CloseDocument();
        }
    }
}